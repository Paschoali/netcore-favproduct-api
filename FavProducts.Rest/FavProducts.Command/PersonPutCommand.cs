using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System.Threading.Tasks;

namespace FavProducts.Command
{
    public class PersonPutCommand : IPersonPutCommand
    {
        private readonly IPersonService _personService;

        public PersonPutCommand(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            return await _personService.UpdateAsync(person);
        }
    }
}