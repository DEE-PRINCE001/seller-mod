using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreCommerce.Infrastructure.Implementations.Search;

public class PostgresSearchService : ISearchService
{
    public IQueryable<Product> SearchProducts(IQueryable<Product> query, string searchTerm)
    {
        var term = searchTerm.Trim().ToLower();
        return query.Where(p => EF.Functions.ILike(p.Name, $"%{term}%") || 
                                 EF.Functions.ILike(p.Description, $"%{term}%"));
    }
}