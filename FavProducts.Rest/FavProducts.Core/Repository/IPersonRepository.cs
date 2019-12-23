using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Repository
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> ListAsync();
        Task<Person> GetAsync(System.Guid personId);
        Task<bool> GetByEmailAsync(string email);
        Task<Person> CreateAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task DeleteAsync(System.Guid personId);
    }
}