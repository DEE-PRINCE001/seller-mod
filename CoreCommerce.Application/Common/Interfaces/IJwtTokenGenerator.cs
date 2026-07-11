using CoreCommerce.Domain.Entities;

namespace CoreCommerce.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}