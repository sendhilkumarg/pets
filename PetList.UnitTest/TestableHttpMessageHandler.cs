using System;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PetList.UnitTest
{
    /// <summary>
    /// HttpMessageHandler with a public SendAsync method that can be mocked and verified with a mocking framework
    /// </summary>
    public class TestableHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        /// This method can be used by mock frameworks to change the behaviour of the protected SendAsync method
        /// </summary>
        public virtual Task<HttpResponseMessage> SendAsyncPublic(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException($"Method: {request.Method}; Uri: {request.RequestUri}");
        }

        /// <summary>
        /// Processes the request
        /// </summary>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsyncPublic(request, cancellationToken);
        }

    }
}
