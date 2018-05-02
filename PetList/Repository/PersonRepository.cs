using PetList.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PetList.Utils;
using PetList.Models;

namespace PetList.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PeopleDomainSettings _options;
        private readonly IHttpClientManager _httpClientManager;

        public PersonRepository(PeopleDomainSettings peopleDomainSettings , IHttpClientManager httpClientManager)
        {
            _options = peopleDomainSettings;
            _httpClientManager = httpClientManager;
        }

        public async Task<IEnumerable<Person>> GetPersons(string petType)
        {

            using (var request = new HttpRequestMessage(HttpMethod.Get, _options.Path))
            {
                using (var response = await _httpClientManager.GetClient().SendAsync(request).ConfigureAwait(true))
                {
                    var persons= await response.Content.ReasAsJsonAsync<IEnumerable<Person>>().ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(petType)){
                        var result = persons.Where(x => x.pets != null).Select(x =>  new Person
                        {
                            age = x.age,
                            name = x.name,
                            gender = x.gender,
                            pets =  x.pets?.Where(y => y.type.Trim().ToUpper().Equals(petType.ToUpper())).ToList(),

                            }
                        );
                        return result;
                    }
                    return persons;
                }
            }
           
        }

        public async Task<IEnumerable<Pet>> GetPets()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, _options.Path))
            {
                using (var response = await _httpClientManager.GetClient().SendAsync(request).ConfigureAwait(false))
                {
                    var persons = await response.Content.ReasAsJsonAsync<IEnumerable<Person>>().ConfigureAwait(false);
                    var pets =  persons.Where(x => x.pets != null).ToList().SelectMany(x=>x.pets);
                    return pets;
                }
            }
        }
    }
}
