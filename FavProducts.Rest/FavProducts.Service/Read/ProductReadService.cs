using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using FavProducts.Domain;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Service.Read
{
    public class ProductReadService : IProductReadService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFlurlClient _flurlClient;
        private readonly int _maxAttempts;
        private readonly ILogger _logger;

        public ProductReadService(
            IProductRepository productRepository, 
            IFlurlClient flurlClient, 
            int maxAttempts, 
            ILogger logger)
        {
            _productRepository = productRepository;
            _flurlClient = flurlClient;
            _maxAttempts = maxAttempts;
            _logger = logger.ForContext<ProductReadService>();
        }

        public async Task<bool> GetAsync(Guid productId, Guid personId) => await _productRepository.GetAsync(productId, personId);

        public async Task<Product> GetProductInfoAsync(Guid productId)
        {
            int attempts = 0;

            while (attempts < _maxAttempts)
            {
                try
                {
                    var response = await _flurlClient.Request(productId.ToString()).GetAsync().ReceiveJson<Product>();

                    return response;
                }
                catch (FlurlHttpException ex)
                {
                    attempts++;
                    _logger.Error($"Error while getting product info from external service. Attempts: {attempts}. Message: {ex.Message}. StackTrace: {ex.StackTrace}.");
                }
            }

            return null;
        }

        public Product GetProductInfo(Guid productId)
        {
            int attempts = 0;

            while (attempts < _maxAttempts)
            {
                try
                {
                    var response = _flurlClient.Request(productId.ToString()).GetAsync().ReceiveJson<Product>().Result;

                    return response;
                }
                catch (FlurlHttpException ex)
                {
                    attempts++;
                    _logger.Error($"Error while getting product info from external service. Attempts: {attempts}. Message: {ex.Message}. StackTrace: {ex.StackTrace}.");
                }
            }

            return null;
        }

        public async Task<IEnumerable<Guid>> ListPersonProductIdsAsync(Guid personId)
        {
            return await _productRepository.ListPersonProductIdsAsync(personId);
        }
    }
}