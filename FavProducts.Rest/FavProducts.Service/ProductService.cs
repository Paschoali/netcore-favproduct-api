using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FavProducts.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductReadService _productReadService;
        private readonly IProductWriteService _productWriteService;
        private readonly object _lock;

        public ProductService(IProductReadService productReadService, IProductWriteService productWriteService)
        {
            _productReadService = productReadService;
            _productWriteService = productWriteService;
            _lock = new object();
        }

        public async Task AddProductAsync(Guid productId, Guid personId)
        {
            Product product = await _productReadService.GetProductAsync(productId);

            if (product == null)
            {
                throw new Exception($"Cannot find this product or it does not exists. Id: {productId}");
            }

            bool productAlreadyAdded = await _productReadService.GetAsync(productId, personId);

            if (productAlreadyAdded)
                throw new Exception("This product is already in this list");

            await _productWriteService.AddAsync(productId, personId);
        }

        public async Task<IEnumerable<Product>> ListPersonProducts(Guid personId)
        {
            IEnumerable<Guid> productIds = await _productReadService.ListPersonProductIdsAsync(personId);

            if (!productIds.Any())
                return new List<Product>();

            var products = new List<Product>();

            var actions = new List<Action>();
            //var taskList = new List<Task>();

            foreach (var productId in productIds)
            {
                actions.Add(() =>
                {
                    Product product = _productReadService.GetProductAsync(productId).Result;

                    if (product != null)
                    {
                        lock (_lock)
                        {
                            products.Add(product);
                        }
                    }
                });

                //taskList.Add(Task.Run(async () =>
                //{
                //    Task<Product> product = _productReadService.GetProductAsync(productId);
                //    await Task.WhenAll(product);

                //    lock (_lock)
                //    {
                //        if (product.Result != null)
                //            products.Add(product.Result);
                //    }
                //}));
            }

            //await Task.WhenAll(taskList);

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