using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAC;
using ContosoUniversity.DAO;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        //private SchoolContext db = new SchoolContext();
        private ICourseRepository CourseRepos { get; set; }
        private IDepartmentRepository DepartRepos { get; set; }

        public CourseController()
        {
            SchoolContext db = new SchoolContext();
            CourseRepos = new EFCourseRepository(db);
            DepartRepos = new EFDepartmentRepository(db);
        }

        public CourseController(ICourseRepository courseRepos, IDepartmentRepository departRepos)
        {
            CourseRepos = courseRepos;
            DepartRepos = departRepos;
        }

        // GET: /Course/
        public ActionResult Index()
        {
            //var courses = db.Courses.Include(c => c.Department);
            //var courses = courses.ToList();
            var courses = CourseRepos.FindAll();
            return View(courses);
        }

        // GET: /Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Course course = db.Courses.Find(id);
            Course course = CourseRepos.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: /Course/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = DepartmentsDropDownList();
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CourseID,Title,Credits,DepartmentID")] Course course)
        {
            try {
                if (ModelState.IsValid) {
                    //db.Courses.Add(course);
                    //db.SaveChanges();
                    CourseRepos.Create(course);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException dex) {
                ModelState.AddModelError("", "unable to save changes");
            }
            ViewBag.DepartmentID = DepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: /Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Course course = db.Courses.Find(id);
            Course course = CourseRepos.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = DepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CourseID,Title,Credits,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(course).State = EntityState.Modified;
                    //db.SaveChanges();
                    CourseRepos.Update(course);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException dex)
            {
                ModelState.AddModelError("", "unable to save changes");
            }
            ViewBag.DepartmentID = DepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: /Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Course course = db.Courses.Find(id);
            Course course = CourseRepos.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Course course = db.Courses.Find(id);
            //db.Courses.Remove(course);
            //db.SaveChanges();
            CourseRepos.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CourseRepos.Dispose();
            }
            base.Dispose(disposing);
        }

        private SelectList DepartmentsDropDownList(object selectedDepartment = null)
        {
            var departments = DepartRepos.FindAll()
                .OrderBy(d => d.Name)
                .Select(d => d);
           return new SelectList(departments, "DepartmentID", "Name", selectedDepartment);
        }
    }
}
