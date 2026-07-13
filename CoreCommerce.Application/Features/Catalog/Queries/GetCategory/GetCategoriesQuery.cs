using CoreCommerce.Application.Features.Catalog.DTOs;
using MediatR;

namespace CoreCommerce.Application.Features.Catalog.Queries;


public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;