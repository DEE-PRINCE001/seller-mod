namespace CoreCommerce.Application.Features.Auth.Commands.Register;

public record RegisterUserResultDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);