using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class ProductListCommand : IProductListCommand
    {
        private readonly IProductService _productService;

        public ProductListCommand(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<Product>> ListPersonProducts(Guid personId)
        {
            return await _productService.ListPersonProducts(personId);
        }
    }
}