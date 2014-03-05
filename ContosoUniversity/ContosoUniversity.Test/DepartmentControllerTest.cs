using System;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContosoUniversity.Models;
using ContosoUniversity.DAO;
using ContosoUniversity.Controllers;

namespace ContosoUniversity.Test
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
    }
}
