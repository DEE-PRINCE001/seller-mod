using CoreCommerce.Application.Features.Auth.Commands.ForgotPassword;
using CoreCommerce.Application.Features.Auth.Commands.Login;
using CoreCommerce.Application.Features.Auth.Commands.RefreshToken;
using CoreCommerce.Application.Features.Auth.Commands.Register;
using CoreCommerce.Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CoreCommerce.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("StrictApiPolicy")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var messageOrToken = await _mediator.Send(command);
        return Ok(new { message = messageOrToken });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password has been updated successfully." });
    }
}