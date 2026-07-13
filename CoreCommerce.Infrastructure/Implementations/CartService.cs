using System.Text.Json;
using CoreCommerce.Domain.Entities.CartEntities;
using CoreCommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CoreCommerce.Infrastructure.Implementations;

public class CartService : ICartService
{
    private readonly IDistributedCache _cache;
    private static readonly DistributedCacheEntryOptions CacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public CartService(IDistributedCache cache)
    {
        _cache = cache;
    }

    private static string GetCacheKey(string cartId) => $"cart:{cartId}";

    public async Task<ShoppingCart?> GetCartAsync(string cartId, CancellationToken cancellationToken)
    {
        var data = await _cache.GetStringAsync(GetCacheKey(cartId), cancellationToken);
        if (data is null) return null;

        return JsonSerializer.Deserialize<ShoppingCart>(data);
    }

    public async Task<ShoppingCart> UpdateCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        var data = JsonSerializer.Serialize(cart);
        await _cache.SetStringAsync(GetCacheKey(cart.CartId), data, CacheOptions, cancellationToken);
        return cart;
    }

    public async Task DeleteCartAsync(string cartId, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(GetCacheKey(cartId), cancellationToken);
    }
}