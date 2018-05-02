using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetList.Models;
using PetList.Repository;

namespace PetList.Controllers
{
    [Produces("application/json")]
    [Route("api/Person")]
    public class PersonController : Controller
    {
        public IPersonRepository _personRepository;
        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet("[action]")]
        public Task<IEnumerable<Person>> GetPersons([FromQuery]string petType)
        {
           return _personRepository.GetPersons(petType);
        }

        [HttpGet("[action]")]
        public Task<IEnumerable<Pet>> GetPets([FromQuery] string gender )
        {
            return _personRepository.GetPets();
        }

    }
}