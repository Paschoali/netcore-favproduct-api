using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IProductListCommand
    {
        Task<IEnumerable<Product>> ListPersonProducts(Guid personId);
    }
}