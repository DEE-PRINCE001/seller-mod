using MediatR;

namespace CoreCommerce.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<RegisterUserResultDto>;