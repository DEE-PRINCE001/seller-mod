using CoreCommerce.Domain.Entities.CartEntities;

namespace CoreCommerce.Application.Common.Interfaces;

public interface ICartService
{
    Task<ShoppingCart?> GetCartAsync(string cartId, CancellationToken cancellationToken);
    Task<ShoppingCart> UpdateCartAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task DeleteCartAsync(string cartId, CancellationToken cancellationToken);
}