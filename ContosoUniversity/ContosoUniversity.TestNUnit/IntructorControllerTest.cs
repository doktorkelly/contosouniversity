using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using ContosoUniversity.DAO;
using System.Web.Mvc;

namespace ContosoUniversity.TestNUnit
{
    [TestFixture]
    class IntructorControllerTest
    {
        [Test]
        public void Index_WithNullIdParams_ReturnsModelWithInstructors()
        {
            //arrange
            var instructorReposMock = new Mock<IRepository<Instructor>>();
            IEnumerable<Instructor> instructors = new List<Instructor>() {
                new Instructor() {ID = 1, FirstMidName = "i1", LastName ="l1"}
            };
            instructorReposMock
                .Setup(repos => repos.FindAll())
                .Returns(instructors);
            var controller = new InstructorController(instructorReposMock.Object, null, null);

            //act
            var result = controller.Index(null, null) as ViewResult;

            //assert
            var viewModel = result.ViewData.Model as InstructorIndexData;
            Assert.AreEqual(1, viewModel.Instructors.Count());
           
        }
    }
}
