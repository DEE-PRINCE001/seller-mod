using MediatR;

namespace CoreCommerce.Application.Features.Catalog.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId
) : IRequest<Guid>;