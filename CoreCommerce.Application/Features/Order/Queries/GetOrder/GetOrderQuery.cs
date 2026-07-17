
using MediatR;

namespace CoreCommerce.Application.Features.Order.Queries.GetOrder;

public record GetOrdersQuery : IRequest<IEnumerable<OrderHistoryDto>>
{
    public Guid? UserId { get; init; }
}