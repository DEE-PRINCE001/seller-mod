namespace CoreCommerce.Application.Features.Catalog.DTOs;

public record CategoryDto(Guid Id,
                            string Name,
                            string Slug);