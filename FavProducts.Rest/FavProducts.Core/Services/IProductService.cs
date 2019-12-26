using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IProductService
    {
        Task AddProductAsync(Guid productId, Guid personId);
        Task<IEnumerable<Product>> ListPersonProducts(Guid personId, int pageNumber);
        Task RemoveFromPersonListAsync(Guid productId, Guid personId);
    }
}