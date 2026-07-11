using CoreCommerce.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CoreCommerce.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}