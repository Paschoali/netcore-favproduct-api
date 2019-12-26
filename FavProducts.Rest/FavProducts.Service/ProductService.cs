using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductReadService _productReadService;
        private readonly IProductWriteService _productWriteService;
        private readonly ILogger _logger;
        private readonly object _lock;

        public ProductService(IProductReadService productReadService, IProductWriteService productWriteService, ILogger logger)
        {
            _productReadService = productReadService;
            _productWriteService = productWriteService;
            _logger = logger.ForContext<ProductService>();
            _lock = new object();
        }

        public async Task AddProductAsync(Guid productId, Guid personId)
        {
            _logger.Debug($"Adding ProductId: {productId} to PersonId: {personId}.");
            Product product = await _productReadService.GetProductInfoAsync(productId);

            if (product == null)
            {
                _logger.Information($"Cannot find this product or it does not exists.Id: { productId}.");
                throw new ArgumentException($"Cannot find this product or it does not exists. Id: {productId}.");
            }

            bool productAlreadyAdded = await _productReadService.GetAsync(productId, personId);

            if (productAlreadyAdded)
            {
                _logger.Information($"This product is already in this list. ProductId: {productId}, PersonId: {personId}.");
                throw new ArgumentException("This product is already in this list.");
            }

            await _productWriteService.AddAsync(productId, personId);
            _logger.Debug($"ProductId {productId} added to PersonId: {personId}.");
        }

        public async Task<IEnumerable<Product>> ListPersonProducts(Guid personId, int pageNumber)
        {
            IEnumerable<Guid> productIds = await _productReadService.ListPersonProductIdsAsync(personId, pageNumber);

            if (!productIds.Any())
            {
                _logger.Debug($"Empty product list. PersonId: {personId}.");
                return new List<Product>();
            }

            var products = new List<Product>();

            var actions = new List<Action>();

            foreach (var productId in productIds)
            {
                actions.Add(() =>
                {
                    _logger.Debug($"Getting ProductId {productId} information from external service.");
                    Product product = _productReadService.GetProductInfoAsync(productId).Result;

                    if (product == null)
                    {
                        _logger.Debug("Can not get product info from external service or it does not exists.");
                        return;
                    }

                    lock (_lock)
                    {
                        products.Add(product);
                    }
                });
            }

            Parallel.Invoke(actions.ToArray());

            actions.Clear();

            return products;
        }

        public async Task RemoveFromPersonListAsync(Guid productId, Guid personId)
        {
            await _productWriteService.RemoveFromPersonListAsync(productId, personId);
        }
    }
}