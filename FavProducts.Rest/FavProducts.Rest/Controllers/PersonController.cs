using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FavProducts.Core.Command;
using FavProducts.Core.Configuration;
using FavProducts.Core.Rest.Resource;
using FavProducts.Core.Rest.Transport;
using FavProducts.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FavProducts.Rest.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
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
        public async Task<IEnumerable<PersonResource>> GetAll([FromQuery]PersonListRequest request)
        {
            var pageNumber = request.Page;
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            IEnumerable<Person> personList = await _personListCommand.ListAsync(pageNumber);

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