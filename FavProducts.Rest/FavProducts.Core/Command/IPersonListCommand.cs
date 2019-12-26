using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IPersonListCommand
    {
        Task<IEnumerable<Person>> ListAsync(int? pageNumber);
    }
}