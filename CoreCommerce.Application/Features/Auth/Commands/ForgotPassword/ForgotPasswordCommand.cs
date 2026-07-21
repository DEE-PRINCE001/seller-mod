
using MediatR;

namespace CoreCommerce.Application.Features.Auth.Commands.ForgotPassword;
public record ForgotPasswordCommand(string Email) : IRequest<string>;