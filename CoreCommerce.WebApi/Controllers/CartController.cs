using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Domain.Entities.CartEntities;
using CoreCommerce.Application.Features.Cart.Commands.UpdateCartItem;

namespace CoreCommerce.WebApi.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICartService _cartService;

    public CartController(IMediator mediator, ICartService cartService)
    {
        _mediator = mediator;
        _cartService = cartService;
    }

    [HttpGet("{cartId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
    public async Task<IActionResult> GetCart(string cartId, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetCartAsync(cartId, cancellationToken) 
                   ?? new ShoppingCart { CartId = cartId };
        return Ok(cart);
    }

    [HttpPut("items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        var updatedCart = await _mediator.Send(command, cancellationToken);
        return Ok(updatedCart);
    }

    [HttpDelete("{cartId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart(string cartId, CancellationToken cancellationToken)
    {
        await _cartService.DeleteCartAsync(cartId, cancellationToken);
        return NoContent();
    }
}