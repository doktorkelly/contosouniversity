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
        private IRepository<Course> CourseRepos { get; set; }
        private IRepository<Department> DepartRepos { get; set; }

        public CourseController()
        {
            SchoolContext db = new SchoolContext();
            CourseRepos = new EFRepository<Course>(db);
            DepartRepos = new EFRepository<Department>(db);
        }

        public CourseController(IRepository<Course> courseRepos, IRepository<Department> departRepos)
        {
            CourseRepos = courseRepos;
            DepartRepos = departRepos;
        }

        // GET: /Course/
        public ActionResult Index()
        {
            IEnumerable<Course> courses = CourseRepos.FindAll();
            return View(courses);
        }

        // GET: /Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            CourseRepos.Remove(id);
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
            IEnumerable<Department> departments = DepartRepos.FindAll()
                .OrderBy(d => d.Name)
                .Select(d => d);
           return new SelectList(departments, "DepartmentID", "Name", selectedDepartment);
        }
    }
}
