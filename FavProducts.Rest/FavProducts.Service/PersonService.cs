using FavProducts.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Service
{
    public class PersonService : IPersonService
    {
        private readonly IPersonReadService _personReadService;
        private readonly IPersonWriteService _personWriteService;

        public PersonService(IPersonReadService personReadService, IPersonWriteService personWriteService)
        {
            _personReadService = personReadService;
            _personWriteService = personWriteService;
        }

        public async Task<IEnumerable<Domain.Person>> ListAsync() => await _personReadService.ListAsync();

        public async Task<Domain.Person> GetAsync(Guid personId) => await _personReadService.GetAsync(personId);

        public async Task<Domain.Person> CreateAsync(Domain.Person person)
        {
            bool personExists = await _personReadService.GetByEmailAsync(person.Email);

            if (personExists) 
                throw new ArgumentException($"Email address {person.Email} is already in use.");

            return await _personWriteService.CreateAsync(person);
        }

        public async Task<Domain.Person> UpdateAsync(Domain.Person person)
        {
            bool personExists = await _personReadService.GetByEmailAsync(person.Email);

            if (personExists)
                throw new ArgumentException($"Email address {person.Email} is already in use.");

            return await _personWriteService.UpdateAsync(person);
        }

        public async Task DeleteAsync(Guid personId) => await _personWriteService.DeleteAsync(personId);
    }
}