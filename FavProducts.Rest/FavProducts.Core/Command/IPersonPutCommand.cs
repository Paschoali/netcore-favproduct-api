using FavProducts.Domain;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IPersonPutCommand
    {
        Task<Person> UpdateAsync(Person person);
    }
}