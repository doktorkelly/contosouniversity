using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAO;

namespace ContosoUniversity.Test
{
    [TestClass]
    public class CourseControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //arrange
            //TODO:
            //ICourseRepository courseRepos = null;
            //IDepartmentRepository departRepos = null;
            //CourseController controller = new CourseController(courseRepos, departRepos);
            CourseController controller = new CourseController();

            //act
            ViewResult result = controller.Index() as ViewResult;

            //assert
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }
    }
}
