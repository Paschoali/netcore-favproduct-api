using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonGetCommand : IPersonGetCommand
    {
        private readonly IPersonService _personService;

        public PersonGetCommand(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<Person> ExecuteAsync(Guid personId)
        {
            return await _personService.GetAsync(personId);
        }
    }
}