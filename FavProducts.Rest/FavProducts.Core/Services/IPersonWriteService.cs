using FavProducts.Domain;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface IPersonWriteService
    {
        Task<Person> CreateAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task DeleteAsync(System.Guid personId);
    }
}