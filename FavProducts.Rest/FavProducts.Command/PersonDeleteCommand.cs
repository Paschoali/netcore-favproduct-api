using FavProducts.Core.Command;
using FavProducts.Core.Services;
using System;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonDeleteCommand : IPersonDeleteCommand
    {
        private readonly IPersonService _personService;

        public PersonDeleteCommand(IPersonService personService)
        {
            _personService = personService;
        }
        public async Task RemoveAsync(Guid personId)
        {
            await _personService.DeleteAsync(personId);
        }
    }
}