using System;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IPersonDeleteCommand
    {
        Task RemoveAsync(Guid personId);
    }
}