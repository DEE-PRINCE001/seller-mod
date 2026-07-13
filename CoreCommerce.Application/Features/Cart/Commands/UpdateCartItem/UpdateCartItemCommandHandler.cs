using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Domain.Entities.CartEntities;

namespace CoreCommerce.Application.Features.Cart.Commands.UpdateCartItem;


public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, ShoppingCart>
{
    private readonly ICartService _cartService;
    private readonly IApplicationDbContext _dbContext;

    public UpdateCartItemCommandHandler(ICartService cartService, IApplicationDbContext dbContext)
    {
        _cartService = cartService;
        _dbContext = dbContext;
    }

    public async Task<ShoppingCart> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        // Validate product existence and stock availability
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException("The requested product does not exist.");

        if (request.Quantity > product.StockQuantity)
            throw new InvalidOperationException($"Insufficient stock. Available: {product.StockQuantity}");

        // Fetch or create the active cart state from Redis
        var cart = await _cartService.GetCartAsync(request.CartId, cancellationToken) 
                   ?? new ShoppingCart { CartId = request.CartId };

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        if (request.Quantity == 0)
        {
            // Remove item from cart if target quantity is set to 0
            if (existingItem != null) cart.Items.Remove(existingItem);
        }
        else
        {
            if (existingItem != null)
            {
                existingItem.Quantity = request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = request.Quantity
                });
            }
        }

        return await _cartService.UpdateCartAsync(cart, cancellationToken);
    }
}