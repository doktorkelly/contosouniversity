using System;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContosoUniversity.Models;
using ContosoUniversity.DAO;
using ContosoUniversity.Controllers;

namespace ContosoUniversity.MSTest.UT
{
    [TestClass]
    public class DepartmentControllerTest
    {
        [TestMethod]
        public void DeleteConfirmed_WithParamId_CallsReposRemove()
        {
            //arrange
            var departReposMock = new Mock<IRepository<Department>>();
            var controller = new DepartmentController(departReposMock.Object, null);

            //act
            var result = controller.DeleteConfirmed(1) as RedirectToRouteResult;

            //assert
            departReposMock.Verify(r => r.Remove(1), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Details()
        {
            //arrange
            var departReposMock = new Mock<IRepository<Department>>();
            var department = new Department() { DepartmentID = 1, Name = "depart01", StartDate = new DateTime(2014, 01, 01) };
            departReposMock
                .Setup(r => r.FindById(1))
                .Returns(department);
            var controller = new DepartmentController(departReposMock.Object, null);

            //act
            var result = controller.Details(1) as ViewResult;

            //assert
            var model = result.ViewData.Model as Department;
            Assert.AreEqual("depart01", model.Name);
        }
    }
}
