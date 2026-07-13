using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Application.Features.Catalog.DTOs;
using CoreCommerce.Application.Features.Catalog.Queries.GetProduct;

namespace CoreCommerce.Application.Catalog.Queries.GetProduct;



public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISearchService _searchService;

    public GetProductsQueryHandler(IApplicationDbContext context, ISearchService searchService)
    {
        _context = context;
        _searchService = searchService;
    }

    public async Task<PagedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking();

        // 1. Filtering
        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm;
            // Simple robust database evaluation (ILIKE equivalent in EF Core via EF.Functions for production performance)
            // query = query.Where(p => EF.Functions.Like(p.Name, $"%{term}%") || 
            //                          EF.Functions.Like(p.Description, $"%{term}%"));
            query = _searchService.SearchProducts(query, term);
        }

        // 2. Sorting
        query = request.SortOrder?.ToLower() switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_desc" => query.OrderByDescending(p => p.Name),
            "name_asc" => query.OrderBy(p => p.Name),
            _ => query.OrderByDescending(p => p.CreatedAtUtc) // Default sorting: Latest arrivals
        };

        // 3. Materialization & Pagination
        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.StockQuantity,
                p.CategoryId,
                p.Category.Name))
            .ToListAsync(cancellationToken);

        return new PagedList<ProductDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}