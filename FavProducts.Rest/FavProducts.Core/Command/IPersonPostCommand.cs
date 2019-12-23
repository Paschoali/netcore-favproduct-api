using FavProducts.Domain;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IPersonPostCommand
    {
        Task<Person> CreateAsync(Person person);
    }
}