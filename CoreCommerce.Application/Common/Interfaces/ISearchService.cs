using CoreCommerce.Domain.Entities;

namespace CoreCommerce.Application.Common.Interfaces;
public interface ISearchService
{
    IQueryable<Product> SearchProducts(IQueryable<Product> query, string searchTerm);
}
