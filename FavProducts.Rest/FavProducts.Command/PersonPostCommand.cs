using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using Serilog;
using System;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonPostCommand : IPersonPostCommand
    {
        private readonly IPersonService _personService;
        private readonly ILogger _logger;

        public PersonPostCommand(IPersonService personService, ILogger logger)
        {
            _personService = personService;
            _logger = logger.ForContext<PersonPostCommand>();
        }

        public async Task<Person> CreateAsync(Person person)
        {
            person.Id = person.Id == default(Guid) ? Guid.NewGuid() : person.Id;

            return await _personService.CreateAsync(person);
        }
    }
}