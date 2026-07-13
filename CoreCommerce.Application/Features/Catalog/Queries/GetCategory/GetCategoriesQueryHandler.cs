using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Features.Catalog.DTOs;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Features.Catalog.Queries;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto(c.Id, c.Name, c.Slug))
            .ToListAsync(cancellationToken);
    }
}