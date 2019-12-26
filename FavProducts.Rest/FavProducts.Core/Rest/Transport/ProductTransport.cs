using System;
using System.ComponentModel.DataAnnotations;

namespace FavProducts.Core.Rest.Transport
{
    public class ProductGetRequest
    {
        [Required]
        public int Page { get; set; }
    }

    public class ProductPostRequest
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}