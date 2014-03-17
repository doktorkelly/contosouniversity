using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ContosoUniversity.Models;

namespace ContosoUniversity.NUTest.UT.Controllers.Models
{
    [TestFixture]
    class CourseTest
    {
        [Test]
        public void Title_SetValue_GetValue()
        {
            //arrange
            Course course = new Course() { CourseID = 1, Title = "title01" };

            //act
            string title = course.Title;

            //assert
            Assert.AreEqual("title01", title);
        }
    }
}
