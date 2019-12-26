using System;
using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IPersonReadService
    {
        Task<IEnumerable<Person>> ListAsync(int pageNumber);
        Task<Person> GetAsync(Guid personId);
        Task<bool> GetByEmailAsync(string email);
    }
}