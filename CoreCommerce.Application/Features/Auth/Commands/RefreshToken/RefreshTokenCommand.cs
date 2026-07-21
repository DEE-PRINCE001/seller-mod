using MediatR;
using CoreCommerce.Application.Features.Auth.Commands.Login;

namespace CoreCommerce.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken, 
    string RefreshToken) : IRequest<AuthResultDto>;