using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using System;
using System.Threading.Tasks;

namespace FavProducts.Service.Write
{
    public class ProductWriteService : IProductWriteService
    {
        private readonly IProductRepository _productRepository;

        public ProductWriteService(IProductRepository productRepository) => _productRepository = productRepository;

        public async Task AddAsync(Guid productId, Guid personId) => await _productRepository.AddAsync(productId, personId);

        public async Task RemoveFromPersonListAsync(Guid productId, Guid personId) => await _productRepository.RemoveFromPersonListAsync(productId, personId);
    }
}