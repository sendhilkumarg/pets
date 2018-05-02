using PetList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetList.Repository
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersons(string petType);
        Task<IEnumerable<Pet>> GetPets();

    }
}
