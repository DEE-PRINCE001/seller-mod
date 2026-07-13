namespace CoreCommerce.Domain.Entities.CartEntities;

public class ShoppingCart
{
    public string CartId { get; set; } = string.Empty; // Maps to UserId or an anonymous Guid
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
}