using MediatR;

namespace CoreCommerce.Application.Features.Catalog.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name, 
    string Slug) : IRequest<Guid>;