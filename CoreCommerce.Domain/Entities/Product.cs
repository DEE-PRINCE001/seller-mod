using CoreCommerce.Domain.Common;

namespace CoreCommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Foreign Key and Relations
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}