using CoreCommerce.Application.Features.Catalog.DTOs;
using MediatR;

namespace CoreCommerce.Application.Features.Catalog.Queries.GetProduct;

public record GetProductsQuery : IRequest<PagedList<ProductDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid? CategoryId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? SearchTerm { get; init; }
    public string? SortOrder { get; init; }
}