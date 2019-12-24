using FavProducts.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Service
{
    public class PersonService : IPersonService
    {
        private readonly IPersonReadService _personReadService;
        private readonly IPersonWriteService _personWriteService;
        private readonly ILogger _logger;

        public PersonService(IPersonReadService personReadService, IPersonWriteService personWriteService, ILogger logger)
        {
            _personReadService = personReadService;
            _personWriteService = personWriteService;
            _logger = logger.ForContext<PersonService>();
        }

        public async Task<IEnumerable<Domain.Person>> ListAsync() => await _personReadService.ListAsync();

        public async Task<Domain.Person> GetAsync(Guid personId) => await _personReadService.GetAsync(personId);

        public async Task<Domain.Person> CreateAsync(Domain.Person person)
        {
            bool personExists = await _personReadService.GetByEmailAsync(person.Email);

            if (!personExists) 
                return await _personWriteService.CreateAsync(person);

            _logger.Information($"Username address { person.Email} is already in use.");
            throw new ArgumentException($"Username address {person.Email} is already in use.");
        }

        public async Task<Domain.Person> UpdateAsync(Domain.Person person)
        {
            bool personExists = await _personReadService.GetByEmailAsync(person.Email);

            if (!personExists)
                return await _personWriteService.UpdateAsync(person);

            _logger.Information($"Username address { person.Email} is already in use.");
            throw new ArgumentException($"Username address {person.Email} is already in use.");
        }

        public async Task DeleteAsync(Guid personId)
        {
            await _personWriteService.DeleteAsync(personId);
            _logger.Information($"PersonId: {personId} removed from database.");
        }
    }
}