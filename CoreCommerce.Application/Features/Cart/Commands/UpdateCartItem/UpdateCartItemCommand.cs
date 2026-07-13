using CoreCommerce.Domain.Entities.CartEntities;
using MediatR;

namespace CoreCommerce.Application.Features.Cart.Commands.UpdateCartItem;

public record UpdateCartItemCommand : IRequest<ShoppingCart>
{
    public string CartId { get; init; } = string.Empty;
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}