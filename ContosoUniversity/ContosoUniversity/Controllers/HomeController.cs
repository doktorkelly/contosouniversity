using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAC;
using ContosoUniversity.DAO;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Student> StudentRepos { get; set; }

        public HomeController()
        {
            SchoolContext db = new SchoolContext();
            StudentRepos = new EFRepository<Student>(db);
        }

        public HomeController(IRepository<Student> studentRepos)
        {
            StudentRepos = studentRepos;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            var model = StudentRepos.FindAll()
                .GroupBy(s => s.EnrollmentDate)
                .Select(group => new EnrollmentDateGroup() {
                    EnrollmentDate = group.Key,
                    StudentCount = group.Count()
                });
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            StudentRepos.Dispose();
            base.Dispose(disposing);
        }
    }
}