using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class PersonGetCommand : IPersonGetCommand
    {
        private readonly IPersonService _personService;
        private readonly ILogger _logger;

        public PersonGetCommand(IPersonService personService, ILogger logger)
        {
            _personService = personService;
            _logger = logger.ForContext<PersonGetCommand>();
        }

        public async Task<Person> ExecuteAsync(Guid personId)
        {
            if (personId == default(Guid))
            {
                _logger.Error($"PersonId cannot be empty.");
                throw new ArgumentException($"personId cannot be empty.");
            }

            return await _personService.GetAsync(personId);
        }
    }
}