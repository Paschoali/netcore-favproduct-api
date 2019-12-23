using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class ProductPostCommand : IProductPostCommand
    {
        private readonly IProductService _productService;

        public ProductPostCommand(IProductService productService)
        {
            _productService = productService;
        }

        public async Task AddProductAsync(Guid productId, Guid personId)
        {
            await _productService.AddProductAsync(productId, personId);
        }
    }
}