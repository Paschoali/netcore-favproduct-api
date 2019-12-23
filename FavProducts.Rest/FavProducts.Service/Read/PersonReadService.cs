using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavProducts.Service.Read
{
    public class PersonReadService : IPersonReadService
    {
        private readonly IPersonRepository _personRepository;

        public PersonReadService(IPersonRepository personRepository) => _personRepository = personRepository;

        public async Task<IEnumerable<Domain.Person>> ListAsync() => await _personRepository.ListAsync();

        public async Task<Domain.Person> GetAsync(Guid personId) => await _personRepository.GetAsync(personId);

        public async Task<bool> GetByEmailAsync(string email) => await _personRepository.GetByEmailAsync(email);
    }
}