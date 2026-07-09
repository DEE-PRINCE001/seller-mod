using CoreCommerce.Domain.Common;
using CoreCommerce.Domain.Enums;

namespace CoreCommerce.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    
    // Refresh Token fields for secure authentication session management
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTimeUtc { get; set; }
}