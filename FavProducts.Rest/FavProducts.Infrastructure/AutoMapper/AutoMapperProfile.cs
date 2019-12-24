using AutoMapper;
using FavProducts.Domain;
using FavProducts.Core.Rest.Transport;

namespace FavProducts.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserRequest>();
            CreateMap<UserRequest, User>();
        }
    }
}