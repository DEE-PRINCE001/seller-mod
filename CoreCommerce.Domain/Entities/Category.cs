using CoreCommerce.Domain.Common;

namespace CoreCommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    // Navigation property for EF Core relational integrity
    public ICollection<Product> Products { get; set; } = new List<Product>();
}