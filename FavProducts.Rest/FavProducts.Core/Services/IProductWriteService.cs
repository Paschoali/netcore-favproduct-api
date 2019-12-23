using System;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IProductWriteService
    {
        Task AddAsync(Guid productId, Guid personId);
        Task RemoveFromPersonListAsync(Guid productId, Guid personId);
    }
}