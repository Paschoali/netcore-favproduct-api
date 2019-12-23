using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonPostCommand : IPersonPostCommand
    {
        private readonly IPersonService _personService;

        public PersonPostCommand(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<Person> CreateAsync(Person person)
        {
            person.Id = person.Id == default(Guid) ? Guid.NewGuid() : person.Id;

            return await _personService.CreateAsync(person);
        }
    }
}