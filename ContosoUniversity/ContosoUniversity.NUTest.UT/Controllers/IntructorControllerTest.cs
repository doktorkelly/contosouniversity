using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Security.Principal;
using System.Globalization;
using System.Collections.Specialized;
using NUnit.Framework;
using Moq;
using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using ContosoUniversity.DAO;
using ContosoUniversity.NUTest.UT.Helpers;


namespace ContosoUniversity.NUTest.UT.Controllers
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

        //[Ignore] //because of missing controllerContext mock
        [Test]
        public void Edit_WithInstructorsAndNoCourses_CallsReposUpdate()
        {
            //arrange
            //repos mock:
            var instructorReposMock = new Mock<IRepository<Instructor>>();
            var instructors = Create2Instructors(new string[] {"name01", "name02"});
            instructorReposMock
                .Setup(r => r.FindAll())
                .Returns(instructors);
            //controller:
            var controller = new InstructorController(instructorReposMock.Object, null, null);
            controller.ControllerContext = ControllerContextFactory.Create(controller);
            controller.ValueProvider = NameValueProviderFactory.Create();
            //parameters:
            FormCollection formCollection = null; //TODO
            string[] selectedCourses = null; //TODO

            //act
            var result = controller.Edit(1, formCollection, selectedCourses) as RedirectToRouteResult;

            //assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            instructorReposMock.Verify(r => r.FindAll(), Times.Once);
            instructorReposMock.Verify(r => r.Update(It.IsAny<Instructor>()), Times.Once);
            instructorReposMock.Verify(r => r.Update(It.Is<Instructor>(i => i.Courses != null)));
        }

        private static List<Instructor> Create2Instructors(string[] names)
        {
            var instructors = new List<Instructor>() {
                new Instructor() { 
                    ID=1, 
                    FirstMidName=names[0], 
                    LastName=names[0], 
                    OfficeAssignment = new OfficeAssignment()
                },
                new Instructor() { 
                    ID=2, 
                    FirstMidName=names[1], 
                    LastName=names[1],
                    OfficeAssignment = new OfficeAssignment()
                }
            };
            return instructors;
        }
    }
}
