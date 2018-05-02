using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetList.Controllers;
using PetList.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetList.UnitTest
{
    [TestClass]
    public class PersonControllerTest
    {
        protected Mock<IPersonRepository> _repository;
        protected PersonController _personController;

        [TestInitialize()]
        public void PersonRepositoryTestInit()
        {
            _repository = new Mock<IPersonRepository>();
            _personController = new PersonController(_repository.Object);
        }

        [TestMethod]
        public void GetPersons_WhenExecuted_InokesRepository()
        {
            _personController.GetPersons(string.Empty);
            _repository.Setup(x => x.GetPersons(It.IsAny<string>()));
            _repository.Verify(x => x.GetPersons(string.Empty),Times.Once);
        }

        [TestMethod]
        public void GetPersons_WhenExecutedWithFilter_InokesRepositoryWithFilter()
        {
            _personController.GetPersons("cat");
            _repository.Setup(x => x.GetPersons(It.IsAny<string>()));
            _repository.Verify(x => x.GetPersons("cat"), Times.Once);
        }
    }
}
