using FavProducts.Core.Command;
using FavProducts.Core.Services;
using System;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class ProductPostCommand : IProductPostCommand
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductPostCommand(IProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger.ForContext<ProductPostCommand>();
        }

        public async Task AddProductAsync(Guid productId, Guid personId)
        {
            await _productService.AddProductAsync(productId, personId);
        }
    }
}