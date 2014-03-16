using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAO;
using ContosoUniversity.Models;
using Moq;
using System.Collections.Generic;

namespace ContosoUniversity.MSTest.UT
{
    [TestClass]
    public class CourseControllerTest
    {
        [TestMethod]
        public void Index_WithCoursesInRepos_ReturnsModelWithCourses()
        {
            //arrange
            var reposCourseMock = new Mock<IRepository<Course>>();
            IEnumerable<Course> courses = new List<Course>() {
                new Course() {CourseID=10, Title="course01", Credits=1},
                new Course() {CourseID=11, Title="course02", Credits=2}
            };
            reposCourseMock
                .Setup(repos => repos.FindAll())
                .Returns(courses);
            CourseController controller = new CourseController(
                reposCourseMock.Object, null);

            //act
            ViewResult result = controller.Index() as ViewResult;

            //assert
            //Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            //Assert.AreEqual("Index", result.ViewName);
            var model = result.ViewData.Model as List<Course>;
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod]
        public void Create_WithCourseParam_CallsReposCreate()
        {
            //arrange
            var reposCourseMock = new Mock<IRepository<Course>>();
            Course course = new Course() {
                CourseID = 10,
                Title = "course01",
                Credits = 2
            };
            CourseController controller = new CourseController(
                reposCourseMock.Object, null);

            //act
            var result = controller.Create(course) as RedirectToRouteResult;

            //assert
            reposCourseMock.Verify(r => r.Create(course), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeleteConfirmed_WithParamId_CallsReposRemove()
        {
            //arrange
            var reposCourseMock = new Mock<IRepository<Course>>();
            var controller = new CourseController(reposCourseMock.Object, null);

            //act
            var result = controller.DeleteConfirmed(1) as RedirectToRouteResult;

            //assert
            reposCourseMock.Verify(r => r.Remove(1), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
