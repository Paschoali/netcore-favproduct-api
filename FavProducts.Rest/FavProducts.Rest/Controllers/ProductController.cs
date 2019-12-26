using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FavProducts.Core.Command;
using FavProducts.Core.Configuration;
using FavProducts.Core.Rest.Resource;
using FavProducts.Core.Rest.Transport;
using FavProducts.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FavProducts.Rest.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductPostCommand _productPostCommand;
        private readonly IProductListCommand _productListCommand;

        public ProductController(IProductPostCommand productPostCommand, IProductListCommand productListCommand)
        {
            _productPostCommand = productPostCommand;
            _productListCommand = productListCommand;
        }

        [Cached(86400)]
        [HttpGet("{personId}")]
        public async Task<IEnumerable<ProductResource>> ListByPerson(Guid personId, [FromQuery]ProductGetRequest request)
        {
            var pageNumber = request.Page;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            IEnumerable<Product> productList = await _productListCommand.ListPersonProducts(personId, pageNumber);

            IEnumerable<ProductResource> productResourceList = productList.Select(p => new ProductResource
            {
                Id = p.Id,
                Title = p.Title,
                Brand = p.Brand,
                Image = p.Image,
                Price = p.Price,
                ReviewScore = p.ReviewScore
            });

            return productResourceList;
        }

        [HttpPost]
        public async Task Post([FromBody]ProductPostRequest request)
        {
            await _productPostCommand.AddProductAsync(request.ProductId, request.PersonId);
        }
    }
}
