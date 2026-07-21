using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Features.Order.Queries.GetOrder;





public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderHistoryDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders.AsNoTracking();

        if (request.UserId.HasValue)
        {
            query = query.Where(o => o.UserId == request.UserId.Value);
        }

        return await query
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderHistoryDto(
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                o.Status.ToString(),
                o.ShippingAddress))
            .ToListAsync(cancellationToken);
    }
}