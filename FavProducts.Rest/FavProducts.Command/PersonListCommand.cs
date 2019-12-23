using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonListCommand : IPersonListCommand
    {
        private readonly IPersonService _personService;

        public PersonListCommand(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<IEnumerable<Person>> ExecuteAsync()
        {
            return await _personService.ListAsync();
        }
    }
}