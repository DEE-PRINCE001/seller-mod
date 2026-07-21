using CoreCommerce.Domain.Enums;
using MediatR;

namespace CoreCommerce.Application.Features.Order.Commands.UpdateOrderStatus;
public record UpdateOrderStatusCommand : IRequest
{
    public Guid OrderId { get; init; }
    public OrderStatus NewStatus { get; init; }
}