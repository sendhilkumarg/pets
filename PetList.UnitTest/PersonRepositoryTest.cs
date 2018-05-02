using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PetList.Config;
using PetList.Models;
using PetList.Repository;
using PetList.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PetList.UnitTest
{
    [TestClass]
    public class PersonRepositoryTest
    {
        protected PersonRepository _repository;
        protected IHttpClientManager _httpClientManager;
        protected PeopleDomainSettings _domainSettings;
        protected System.Net.Http.HttpClient _httpClient;
        protected Mock<TestableHttpMessageHandler> _mockHandler;
        protected Mock<IHttpContextAccessor> _mockAccessor;
        private readonly string _baseAddress;
        public  PersonRepositoryTest()
        {
            _baseAddress = "http://localhost/";
        }

        [TestInitialize()]
        public  void PersonRepositoryTestInit()
        {
            _domainSettings = new PeopleDomainSettings()
            {
                Path = "api/getperson",
            };

        }
        [TestMethod]
        public void GetPersons_WhenNoFilterForPetTypeSpecified_And_ExternalAPIReturnsData_ReturnsPersonsWithAllPetTypes()
        {
            _mockHandler = new Mock<TestableHttpMessageHandler> { CallBase = true };

            _httpClient = new System.Net.Http.HttpClient(_mockHandler.Object) {
                BaseAddress = new Uri(_baseAddress)
            };
            _httpClientManager = Mock.Of<IHttpClientManager>(x => x.GetClient() == _httpClient);
            _repository = new PersonRepository(_domainSettings, _httpClientManager
            );
            var expression = _mockHandler.CreateSendAsyncExpression(HttpMethod.Get,  $"{_baseAddress}{_domainSettings.Path}");

            var result = CreateMockData();
            _mockHandler.Setup(expression).RespondsWith(result);


           var response = _repository.GetPersons(string.Empty).GetAwaiter().GetResult(); ;
            Assert.IsNotNull(response);
            JsonCompare(result, response, "The result is not expected");

        }

        [TestMethod]
        public void GetPersons_WhenFilterForPetTypeSpecified_And_ExternalAPIReturnsData_ReturnsPersonsWithPetsFilteredByType()
        {
            _mockHandler = new Mock<TestableHttpMessageHandler> { CallBase = true };

            _httpClient = new System.Net.Http.HttpClient(_mockHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _httpClientManager = Mock.Of<IHttpClientManager>(x => x.GetClient() == _httpClient);
            _repository = new PersonRepository(_domainSettings, _httpClientManager
            );
            var expression = _mockHandler.CreateSendAsyncExpression(HttpMethod.Get, $"{_baseAddress}{_domainSettings.Path}");

            var result = CreateMockData();
            _mockHandler.Setup(expression).RespondsWith(result);


            var response = _repository.GetPersons("Cat").GetAwaiter().GetResult(); ;
            Assert.IsNotNull(response);
            JsonCompare(CreateMockResponseDataWithFilter(), response, "The result is not expected");

        }

        //TODO: fix this test case
        [TestMethod]
        [Ignore]
        //[ExpectedHttpStatusCode(HttpStatusCode.BadRequest)]
        public void GetPersons_WhenExternalAPIReturnsHTTPBadRequest_ReturnsResponse()
        {
            _mockHandler = new Mock<TestableHttpMessageHandler> { CallBase = true };

            _httpClient = new System.Net.Http.HttpClient(_mockHandler.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _httpClientManager = Mock.Of<IHttpClientManager>(x => x.GetClient() == _httpClient);
            _repository = new PersonRepository(_domainSettings, _httpClientManager
            );
            var expression = _mockHandler.CreateSendAsyncExpression(HttpMethod.Get, $"{_baseAddress}{_domainSettings.Path}");

            var assume = CreateMockData();
            var badRequestResult = CreateHttpResponseMessage(HttpStatusCode.InternalServerError, assume);

            _mockHandler.Setup(expression).RespondsWith(badRequestResult);

            try
            {
                var response = _repository.GetPersons(string.Empty);//.GetAwaiter().GetResult(); ;
            }
            catch(Exception ex)
            {
                if(ex is HttpRequestException)
                {
                    var statusCode = (ex as HttpRequestException);
                }

            }
           // Assert.Fail("HTTP 500 was not thrown");


        }

        private List<Person> CreateMockData()
        {
            return  new List<Person>()
            {
                new Person()
                {
                    name ="sam",
                    gender = "male",
                    age = 20,
                    pets= new List<Pet>
                    {
                        new Pet()
                        {
                            name = "cutie",
                            type ="cat"
                        },
                        new Pet()
                        {
                            name = "cutie",
                            type ="dog"
                        },
                    }
                },
                new Person()
                {
                    name ="lie",
                    gender = "female",
                    age = 22,
                    pets= new List<Pet>
                    {
                        new Pet()
                        {
                            name = "pet",
                            type ="dog"
                        },
                    }
                },
            };
        }

        private List<Person> CreateMockResponseDataWithFilter()
        {
            return new List<Person>()
            {
                new Person()
                {
                    name ="sam",
                    gender = "male",
                    age = 20,
                    pets= new List<Pet>
                    {
                        new Pet()
                        {
                            name = "cutie",
                            type ="cat"
                        },
                    }
                },
                new Person()
                {
                    name ="lie",
                    gender = "female",
                    age = 22,
                    pets= new List<Pet>
                    {

                    }
                },
            };

        }

        public void JsonCompare(object expected, object actual, string message = "")
        {
            var jsonExpected = JsonConvert.SerializeObject(expected);
            var jsonActual = JsonConvert.SerializeObject(actual);
            if (!jsonExpected.Equals(jsonActual))
                throw new AssertFailedException(
                    $"{message}" +
                    $"{Environment.NewLine}Expected: {jsonExpected}." +
                    $"{Environment.NewLine}Actual: {jsonActual}.");
        }

        /// <summary>
        /// Create HttpResponseMessage object that use to render to client
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="stringContent"></param>
        /// <returns></returns>
        public HttpResponseMessage CreateHttpResponseMessage<T>(HttpStatusCode httpStatusCode, T obj)
        {
            return new HttpResponseMessage(httpStatusCode) { Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8) };
        }

    }
}
