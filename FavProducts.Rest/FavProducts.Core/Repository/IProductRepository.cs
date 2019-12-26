using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Repository
{
    public interface IProductRepository
    {
        Task<bool> GetAsync(Guid productId, Guid personId);
        Task AddAsync(Guid productId, Guid personId);
        Task<IEnumerable<Guid>> ListPersonProductIdsAsync(Guid personId, int pageNumber, int pageSize);
        Task RemoveFromPersonListAsync(Guid productId, Guid personId);
    }
}