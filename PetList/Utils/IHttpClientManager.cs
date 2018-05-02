using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetList.Utils
{
    public interface IHttpClientManager : IDisposable
    {
        HttpClient GetClient();
    }
}
