using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class PersonPutCommand : IPersonPutCommand
    {
        private readonly IPersonService _personService;
        private readonly ILogger _logger;

        public PersonPutCommand(IPersonService personService, ILogger logger)
        {
            _personService = personService;
            _logger = logger.ForContext<PersonPutCommand>();
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            return await _personService.UpdateAsync(person);
        }
    }
}