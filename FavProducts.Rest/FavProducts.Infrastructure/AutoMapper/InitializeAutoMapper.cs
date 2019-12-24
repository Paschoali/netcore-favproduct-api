using System;
using System.Collections.Generic;
using System.Text;
using FavProducts.Core.Rest.Transport;
using FavProducts.Domain;

namespace FavProducts.Infrastructure.AutoMapper
{
    public static class InitializeAutoMapper
    {
        public static void Initialize()
        {
            var autoMapperProfile = new AutoMapperProfile();
        }
    }
}