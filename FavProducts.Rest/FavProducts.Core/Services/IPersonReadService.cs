using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IPersonReadService
    {
        Task<IEnumerable<Person>> ListAsync();
        Task<Person> GetAsync(System.Guid personId);
        Task<bool> GetByEmailAsync(string email);
    }
}