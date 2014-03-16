using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContosoUniversity;
using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using ContosoUniversity.DAO;

namespace ContosoUniversity.MSTest.UT
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_WithoutParams_ReturnsView()
        {
            //arrange
            HomeController controller = new HomeController();

            //act
            ViewResult result = controller.Index() as ViewResult;

            //assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }

        [TestMethod]
        public void About_WithStudentsInRepos_ReturnsModelWithStudents()
        {
            //arrange
            var reposStudentMock = new Mock<IRepository<Student>>();
            IEnumerable<Student> students = new List<Student>() {
                new Student() {ID=1, LastName="stud01", FirstMidName="first01", EnrollmentDate=new DateTime(2014,03,01)},
                new Student() {ID=2, LastName="stud02", FirstMidName="first02", EnrollmentDate=new DateTime(2014,03,02)},
                new Student() {ID=3, LastName="stud03", FirstMidName="first03", EnrollmentDate=new DateTime(2014,03,02)}
            };
            reposStudentMock
                .Setup(r => r.FindAll())
                .Returns(students);
            var controller = new HomeController(reposStudentMock.Object);
            
            //act
            var result = controller.About() as ViewResult;

            //assert
            var model = result.ViewData.Model as IEnumerable<EnrollmentDateGroup>;
            Assert.AreEqual(2, model.Count());
            Assert.AreEqual(1, model.ElementAt(0).StudentCount); //group with 1 in 2014.03.01
            Assert.AreEqual(2, model.ElementAt(1).StudentCount); //group with 2 in 2014.03.02
        }
    }
}
