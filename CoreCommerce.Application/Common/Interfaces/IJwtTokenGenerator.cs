using CoreCommerce.Domain.Entities;
using System.Security.Claims;

namespace CoreCommerce.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}