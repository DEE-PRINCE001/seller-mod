using CoreCommerce.Domain.Entities;
using CoreCommerce.Domain.Entities.OrderEntities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreCommerce.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}