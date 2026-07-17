using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Domain.Entities;
using CoreCommerce.Domain.Enums;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreComerce.Application.Orders.Commands.Checkout;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;

    public CheckoutCommandHandler(IApplicationDbContext context, ICartService cartService, IPaymentService paymentService)
    {
        _context = context;
        _cartService = cartService;
        _paymentService = paymentService;
    }

    public async Task<Guid> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        // 1. Retrieve the cart from Redis
        var cart = await _cartService.GetCartAsync(request.CartId, cancellationToken);
        if (cart == null || !cart.Items.Any())
        {
            throw new InvalidOperationException("Cannot checkout an empty shopping cart.");
        }

        // Use EF execution strategy for resilient PostgreSQL connections during transactions
        using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                UserId = request.UserId,
                ShippingAddress = request.ShippingAddress,
                TotalAmount = cart.TotalAmount,
                Status = OrderStatus.Pending
            };

            foreach (var cartItem in cart.Items)
            {
                // Lock the product row strictly to prevent stock calculation drift (Race Conditions)
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId, cancellationToken);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product {cartItem.ProductName} is no longer available.");
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}. Requested: {cartItem.Quantity}, Available: {product.StockQuantity}");
                }

                // Decrement stock levels immediately inside the database transaction
                product.StockQuantity -= cartItem.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            // 2. Process mock/integrated payment
            var isPaymentSuccessful = await _paymentService.ProcessPaymentAsync(
                orderId, 
                order.TotalAmount, 
                request.PaymentToken, 
                cancellationToken
            );

            if (!isPaymentSuccessful)
            {
                throw new InvalidOperationException("Payment processing failed. Order creation rolled back.");
            }

            // Payment succeeded: Advance state machine directly to Paid
            order.Status = OrderStatus.Paid;
            await _context.SaveChangesAsync(cancellationToken);

            // Commit the structural changes
            await transaction.CommitAsync(cancellationToken);

            // 3. Clear Redis cart state since processing completed successfully
            await _cartService.DeleteCartAsync(request.CartId, cancellationToken);

            return orderId;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}