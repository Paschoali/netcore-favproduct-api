using FavProducts.Core.Command;
using FavProducts.Core.Services;
using System;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class PersonDeleteCommand : IPersonDeleteCommand
    {
        private readonly IPersonService _personService;
        private readonly ILogger _logger;

        public PersonDeleteCommand(IPersonService personService, ILogger logger)
        {
            _personService = personService;
            _logger = logger.ForContext<PersonDeleteCommand>();
        }
        public async Task RemoveAsync(Guid personId)
        {
            await _personService.DeleteAsync(personId);
        }
    }
}