namespace CoreCommerce.Application.Features.Order.Queries.GetOrder;

public record OrderHistoryDto(
    Guid OrderId, 
    DateTime OrderDate, 
    decimal TotalAmount, 
    string Status, 
    string ShippingAddress);