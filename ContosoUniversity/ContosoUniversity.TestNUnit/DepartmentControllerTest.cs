using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;
using System.Linq;
using NUnit.Framework;
using Moq;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAO;
using ContosoUniversity.Models;

namespace ContosoUniversity.TestNUnit
{
    [TestFixture]
    public class DepartmentControllerTest
    {
        [Test]
        public void Details()
        {
            //arrange
            var departReposMock = new Mock<IRepository<Department>>();
            var department = new Department() { 
                DepartmentID = 1, 
                Name = "depart01", 
                StartDate = new DateTime(2014, 01, 01) 
            };
            departReposMock
                .Setup(r => r.FindById(1))
                .Returns(department);
            var controller = new DepartmentController(departReposMock.Object, null);

            //act
            var result = controller.Details(1) as ViewResult;

            //assert
            var model = result.ViewData.Model as Department;
            Assert.AreEqual("depart01", model.Name);
            Assert.AreEqual(1, model.DepartmentID);
            Assert.AreEqual(new DateTime(2014, 01, 01), model.StartDate);
        }
    }
}
