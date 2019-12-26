using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class ProductListCommand : IProductListCommand
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductListCommand(IProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger.ForContext<ProductListCommand>();
        }

        public async Task<IEnumerable<Product>> ListPersonProducts(Guid personId, int pageNumber)
        {
            return await _productService.ListPersonProducts(personId, pageNumber);
        }
    }
}