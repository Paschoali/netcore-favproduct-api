using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IProductReadService
    {
        Task<bool> GetAsync(System.Guid productId, System.Guid personId);
        Task<Product> GetProductAsync(System.Guid productId);
        Task<IEnumerable<System.Guid>> ListPersonProductIdsAsync(System.Guid personId);
    }
}