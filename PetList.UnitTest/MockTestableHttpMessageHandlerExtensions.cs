using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace PetList.UnitTest
{
    public static class MockTestableHttpMessageHandlerExtensions
    {
        /// <summary>
        /// Creates an expression for SendAsync calls that can be used in Setup and Verify methods
        /// </summary>
        /// <param name="mock"></param>
        /// <returns></returns>
        public static Expression<Func<TestableHttpMessageHandler, Task<HttpResponseMessage>>> CreateSendAsyncExpression(this Mock<TestableHttpMessageHandler> mock)
            => mock.CreateSendAsyncExpression(request => true);

        /// <summary>
        /// Create an expression for SendAsync calls that can be used in Setup and Verify methods
        /// </summary>
        /// <param name="mock"></param>
        /// <param name="method">The Http request method</param>
        /// <param name="uri">The Request Uri</param>
        /// <returns></returns>
        public static Expression<Func<TestableHttpMessageHandler, Task<HttpResponseMessage>>> CreateSendAsyncExpression(this Mock<TestableHttpMessageHandler> mock,
            HttpMethod method, string uri)
            => mock.CreateSendAsyncExpression(request => request.Method == method && request.RequestUri.ToString() == uri);

        /// <summary>
        /// Creates an expression for SendAsync calls that can be used in Setup and Verify methods
        /// </summary>
        /// <param name="mock"></param>
        /// <param name="match">Matcher for HttpRequestMessage</param>
        /// <returns></returns>
        public static Expression<Func<TestableHttpMessageHandler, Task<HttpResponseMessage>>> CreateSendAsyncExpression(this Mock<TestableHttpMessageHandler> mock,
            Expression<Func<HttpRequestMessage, bool>> match)
            => handler => handler.SendAsyncPublic(It.Is(match), It.IsAny<CancellationToken>());

        /// <summary>
        /// Sets up a response with status code 200 and an empty body for SendAsync
        /// </summary>
        /// <param name="mock"></param>
        /// <returns></returns>
        public static IReturnsResult<TestableHttpMessageHandler> RespondsWithDefault(this IReturns<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock)
            => mock.RespondsWith(HttpStatusCode.OK);

        /// <summary>
        /// Sets up a response with an empty body for SendAsync
        /// </summary>
        /// <param name="mock"></param>
        /// <param name="statusCode">The response status code</param>
        /// <returns></returns>
        public static IReturnsResult<TestableHttpMessageHandler> RespondsWith(this IReturns<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock,
            HttpStatusCode statusCode)
            => mock.RespondsWith(statusCode, (object)null);

        /// <summary>
        /// Sets up a response with status code 200 for SendAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mock"></param>
        /// <param name="responseBody">Object to be rendered as Json in the response body</param>
        /// <returns></returns>
        public static IReturnsResult<TestableHttpMessageHandler> RespondsWith<T>(this IReturns<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock,
            T responseBody)
            => mock.RespondsWith(HttpStatusCode.OK, responseBody);

        /// <summary>
        /// Sets up a response for SendAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mock"></param>
        /// <param name="statusCode">The response status code</param>
        /// <param name="responseBody">Object to be rendered as Json in the response body</param>
        /// <returns></returns>
        public static IReturnsResult<TestableHttpMessageHandler> RespondsWith<T>(this IReturns<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock,
            HttpStatusCode statusCode, T responseBody)
            => mock.Returns(async (HttpRequestMessage request, CancellationToken token) => {
                var response = request.CreateResponse(statusCode);

                if (responseBody != null)
                {
                    // Convert the input object into a stream and use this stream as the reponse's content.
                    // This simulates real Http responses and helps detect deserialisation errors early

                    var objectContent = new ObjectContent<T>(responseBody, new JsonMediaTypeFormatter());
                    var stream = await objectContent.ReadAsStreamAsync();

                    var streamContent = new StreamContent(stream);
                    streamContent.Headers.ContentType = objectContent.Headers.ContentType;

                    response.Content = streamContent;
                }

                return response;
            });

        /// <summary>
        /// Reads and stores the request for assertion later.
        /// This must be done during Setup, otherwise the HttpClient will dispose the request content after sending the request.
        /// </summary>
        /// <param name="mock"></param>
        /// <param name="processRequest">Contains the delegate to process the request and requestBody.</param>
        /// <returns></returns>
        public static IReturnsThrows<TestableHttpMessageHandler, Task<HttpResponseMessage>> CaptureRequest(this ICallback<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock,
            out Action<Action<HttpRequestMessage, string>> processRequest)
        {
            bool captured = false;
            HttpRequestMessage request = null;
            string requestBody = null;

            var result = mock.Callback((HttpRequestMessage r, CancellationToken c) =>
            {
                captured = true;
                request = r;
                if (r.Content != null)
                {
                    requestBody = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            });

            processRequest = action =>
            {
                if (!captured)
                {
                    throw new InvalidOperationException("Method has not been called.");
                }
                action(request, requestBody);
            };

            return result;
        }

        /// <summary>
        /// Sets up a response for SendAsync
        /// </summary>
        /// <param name="mock"></param>
        /// <param name="httpResponseMessage">HttpResponseMessage object that is render to client</param>
        /// <returns></returns>
        public static IReturnsResult<TestableHttpMessageHandler> RespondsWith(this IReturns<TestableHttpMessageHandler, Task<HttpResponseMessage>> mock,
            HttpResponseMessage httpResponseMessage)
            => mock.Returns(Task.FromResult(httpResponseMessage));


    }
}
