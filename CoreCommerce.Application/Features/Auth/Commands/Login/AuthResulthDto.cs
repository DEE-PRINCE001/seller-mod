namespace CoreCommerce.Application.Features.Auth.Commands.Login;

public record AuthResultDto(
    Guid UserId,
    string Email,
    string AccessToken,
    string RefreshToken
);