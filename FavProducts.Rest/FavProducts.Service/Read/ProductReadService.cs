using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using FavProducts.Domain;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Service.Read
{
    public class ProductReadService : IProductReadService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFlurlClient _flurlClient;
        private readonly int _maxAttempts;

        public ProductReadService(IProductRepository productRepository, IFlurlClient flurlClient, int maxAttempts)
        {
            _productRepository = productRepository;
            _flurlClient = flurlClient;
            _maxAttempts = maxAttempts;
        }

        public async Task<bool> GetAsync(Guid productId, Guid personId) => await _productRepository.GetAsync(productId, personId);

        public async Task<Product> GetProductAsync(Guid productId)
        {
            Product response = null;
            int attempts = 0;

            while (attempts < _maxAttempts)
            {
                try
                {
                    response = await _flurlClient.Request(productId.ToString()).GetAsync().ReceiveJson<Product>();

                    return response;
                }
                catch (FlurlHttpException ex)
                {
                    attempts++;
                    //response = await _flurlClient.Request(productId.ToString()).GetAsync().ReceiveJson<Product>();

                    //return response;
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