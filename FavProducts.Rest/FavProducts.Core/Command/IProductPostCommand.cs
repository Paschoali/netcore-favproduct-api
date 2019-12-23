using System;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IProductPostCommand
    {
        Task AddProductAsync(Guid productId, Guid personId);
    }
}