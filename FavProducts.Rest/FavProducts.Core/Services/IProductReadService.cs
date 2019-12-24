using System;
using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IProductReadService
    {
        Task<bool> GetAsync(Guid productId, Guid personId);
        Task<Product> GetProductInfoAsync(Guid productId);
        Product GetProductInfo(Guid productId);
        Task<IEnumerable<Guid>> ListPersonProductIdsAsync(Guid personId);
    }
}