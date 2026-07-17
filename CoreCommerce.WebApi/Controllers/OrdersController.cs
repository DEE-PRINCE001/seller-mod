using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreCommerce.Domain.Enums;
using CoreCommerce.Application.Features.Order.Queries.GetOrder;
using CoreCommerce.Application.Features.Order.Commands.UpdateOrderStatus;
using CoreComerce.Application.Features.Order.Commands.Checkout;

namespace CoreCommerce.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request, CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var command = new CheckoutCommand
        {
            UserId = userId,
            CartId = request.CartId,
            ShippingAddress = request.ShippingAddress,
            PaymentToken = request.PaymentToken
        };

        var orderId = await _mediator.Send(command, cancellationToken);
        return Ok(orderId);
    }

    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderHistoryDto>))]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetOrdersQuery { UserId = userId }, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderHistoryDto>))]
    public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateOrderStatusCommand { OrderId = id, NewStatus = request.Status }, cancellationToken);
        return NoContent();
    }
}

public record CheckoutRequest(string CartId, string ShippingAddress, string PaymentToken);
public record UpdateStatusRequest(OrderStatus Status);