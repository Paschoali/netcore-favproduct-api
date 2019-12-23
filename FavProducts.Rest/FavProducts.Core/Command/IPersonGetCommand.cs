using FavProducts.Domain;
using System;
using System.Threading.Tasks;

namespace FavProducts.Core.Command
{
    public interface IPersonGetCommand
    {
        Task<Person> ExecuteAsync(Guid personId);
    }
}