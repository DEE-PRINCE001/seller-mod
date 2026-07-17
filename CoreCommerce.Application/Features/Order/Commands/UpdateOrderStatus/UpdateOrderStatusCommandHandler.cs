using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Domain.Enums;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Features.Order.Commands.UpdateOrderStatus;


public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        // Validate state transitions
        bool isValidTransition = (order.Status, request.NewStatus) switch
        {
            (OrderStatus.Pending, OrderStatus.Paid) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Paid, OrderStatus.Processing) => true,
            (OrderStatus.Paid, OrderStatus.Cancelled) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            _ => false
        };

        if (!isValidTransition)
        {
            throw new InvalidOperationException($"Invalid status transition from {order.Status} to {request.NewStatus}.");
        }

        order.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);
    }
}