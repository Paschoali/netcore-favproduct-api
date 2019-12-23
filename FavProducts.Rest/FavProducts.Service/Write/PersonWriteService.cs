using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using System;
using System.Threading.Tasks;

namespace FavProducts.Service.Write
{
    public class PersonWriteService : IPersonWriteService
    {
        private readonly IPersonRepository _personRepository;

        public PersonWriteService(IPersonRepository personRepository) => _personRepository = personRepository;

        public async Task<Domain.Person> CreateAsync(Domain.Person person) => await _personRepository.CreateAsync(person);

        public async Task<Domain.Person> UpdateAsync(Domain.Person person) => await _personRepository.UpdateAsync(person);

        public async Task DeleteAsync(Guid personId) => await _personRepository.DeleteAsync(personId);
    }
}