using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetList.Utils
{
    public class HttpClientManager : IHttpClientManager
    {
        public void Dispose()
        {
        }

        public HttpClient GetClient()
        {
            return new HttpClient();
        }

        //// This code added to correctly implement the disposable pattern.
        //public void Dispose()
        //{
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    Dispose(true);
        //    // uncomment the following line if the finalizer is overridden above.
        //    // GC.SuppressFinalize(this);
        //}
    }
}
