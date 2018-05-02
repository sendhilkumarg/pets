using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetList.Config;
using PetList.Models;
using PetList.Repository;
using PetList.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PetList.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        protected PersonRepository _repository;
        protected IHttpClientManager _httpClientManager;
        protected PeopleDomainSettings _domainSettings;
        protected System.Net.Http.HttpClient _httpClient;
        protected Mock<TestableHttpMessageHandler> _mockHandler;
        protected Mock<IHttpContextAccessor> _mockAccessor;
        public virtual void Init()
        {
            _httpClientManager = Mock.Of<IHttpClientManager>();
           
        }
        [TestMethod]
        public void TestMethod1()
        {
            var domainSettings = new PeopleDomainSettings()
            {
                Path = "api/getperson",
            };

            _mockHandler = new Mock<TestableHttpMessageHandler> { CallBase = true };

            _httpClient = new System.Net.Http.HttpClient(_mockHandler.Object) {
                BaseAddress = new Uri("http://localhost/")
            };
            _httpClientManager = Mock.Of<IHttpClientManager>(x => x.GetClient() == _httpClient);
            _repository = new PersonRepository(domainSettings, _httpClientManager
            );
            var expression = _mockHandler.CreateSendAsyncExpression(HttpMethod.Get,  $"http://localhost/api/getperson");

            var result = new List<Person>()
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
                        }
                    }
                }
            };
            _mockHandler.Setup(expression).RespondsWith(result);

            //var expression = _mockHandler.CreateSendAsyncExpression(HttpMethod.Get, uri);
            //_mockHandler.Setup(expression).RespondsWith("");

            _repository.GetPersons("");
        }
    }
}
