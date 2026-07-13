namespace CoreCommerce.Application.Features.Catalog.DTOs;

public record ProductDto(
    Guid Id, 
    string Name, 
    string Description, 
    decimal Price, 
    int StockQuantity, 
    Guid CategoryId, 
    string CategoryName);