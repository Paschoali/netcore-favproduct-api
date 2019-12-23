using System;

namespace FavProducts.Core.Rest.Resource
{
    public class ProductResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public double ReviewScore { get; set; }
    }
}