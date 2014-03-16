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

namespace ContosoUniversity.NUTest.UT
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

        [Ignore] //because of missing controllerContext mock
        [Test]
        public void Edit_WithInstructorsAndNoCourses_CallsReposUpdate()
        {
            //arrange
            var instructorReposMock = new Mock<IRepository<Instructor>>();
            var instructors = new List<Instructor>() {
                new Instructor() { ID=1, FirstMidName="fmn01", LastName="ln01"},
                new Instructor() { ID=2, FirstMidName="fmn02", LastName="ln02"}
            };
            instructorReposMock
                .Setup(r => r.FindAll())
                .Returns(instructors);
            var controller = new InstructorController(instructorReposMock.Object, null, null);
            FormCollection formCollection = null; //TODO
            string[] selectedCourses = null; //TODO

            //act
            var result = controller.Edit(1, formCollection, selectedCourses) as RedirectToRouteResult;

            //assert
            Assert.AreEqual("index", result.RouteValues["action"]);
            instructorReposMock.Verify(r => r.FindAll(), Times.Once);
            instructorReposMock.Verify(r => r.Update(It.IsAny<Instructor>()), Times.Once);
            instructorReposMock.Verify(r => r.Update(It.Is<Instructor>(i => i.Courses != null)));
        }
    }
}
