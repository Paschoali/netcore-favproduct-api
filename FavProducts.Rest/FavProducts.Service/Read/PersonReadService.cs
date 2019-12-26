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
        private readonly int _pageSize;

        public PersonReadService(IPersonRepository personRepository, int pageSize)
        {
            _personRepository = personRepository;
            _pageSize = pageSize;
        }

        public async Task<IEnumerable<Domain.Person>> ListAsync(int pageNumber) => await _personRepository.ListAsync(pageNumber, _pageSize);

        public async Task<Domain.Person> GetAsync(Guid personId) => await _personRepository.GetAsync(personId);

        public async Task<bool> GetByEmailAsync(string email) => await _personRepository.GetByEmailAsync(email);
    }
}