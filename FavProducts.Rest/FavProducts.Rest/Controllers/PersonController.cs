using FavProducts.Core.Command;
using FavProducts.Core.Configuration;
using FavProducts.Core.Rest.Resource;
using FavProducts.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FavProductsAPI.Controllers
{
    [Route("/person")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonListCommand _personListCommand;
        private readonly IPersonGetCommand _personGetCommand;
        private readonly IPersonPostCommand _personPostCommand;
        private readonly IPersonPutCommand _personPutCommand;
        private readonly IPersonDeleteCommand _personDeleteCommand;

        public PersonController(
            IPersonListCommand personListCommand,
            IPersonGetCommand personGetCommand,
            IPersonPostCommand personPostCommand,
            IPersonPutCommand personPutCommand,
            IPersonDeleteCommand personDeleteCommand)
        {
            _personListCommand = personListCommand;
            _personGetCommand = personGetCommand;
            _personPostCommand = personPostCommand;
            _personPutCommand = personPutCommand;
            _personDeleteCommand = personDeleteCommand;
        }

        [HttpGet]
        [Cached(3600)]
        public async Task<IEnumerable<PersonResource>> GetAll()
        {
            IEnumerable<Person> personList = await _personListCommand.ExecuteAsync();

            IEnumerable<PersonResource> personResourceList = personList.ToList().Select(p => new PersonResource { Id = p.Id, Name = p.Name, Email = p.Email });

            return personResourceList;
        }

        [HttpGet("{personId}")]
        [Cached(3600)]
        public async Task<PersonResource> Get(Guid personId)
        {
            Person person = await _personGetCommand.ExecuteAsync(personId);

            return new PersonResource
            {
                Name = person.Name,
                Email = person.Email
            };
        }

        [HttpPost]
        public async Task<PersonResource> Post([FromBody]Person person)
        {
            person = await _personPostCommand.CreateAsync(person);

            return new PersonResource
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email
            };
        }

        [HttpPut("{personId}")]
        public async Task<PersonResource> Put(Guid personId, [FromBody]Person person)
        {
            person.Id = personId;
            person = await _personPutCommand.UpdateAsync(person);

            return new PersonResource
            {
                Name = person.Name,
                Email = person.Email
            };
        }

        [HttpDelete("{personId}")]
        public async Task<PersonResource> Delete(Guid personId)
        {
            await _personDeleteCommand.RemoveAsync(personId);

            return new PersonResource
            {
                Id = personId
            };
        }
    }
}