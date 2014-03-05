using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAO;
using ContosoUniversity.DAC;

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private IRepository<Department> DepartRepos { get; set; }
        private IRepository<Instructor> InstructorRepos { get; set; }

        public DepartmentController()
        {
            SchoolContext db = new SchoolContext();
            DepartRepos = new EFRepository<Department>(db);
            InstructorRepos = new EFRepository<Instructor>(db);
        }

        public DepartmentController(IRepository<Department> departRepos, IRepository<Instructor> instructorRepos)
        {
            DepartRepos = departRepos;
            InstructorRepos = instructorRepos;
        }

        // GET: /Department/
        public ActionResult Index()
        {
            //var db = new SchoolContext();
            //var departments0 = db.Departments.Include(d => d.Administrator);
            var departments = DepartRepos.FindAll();
            return View(departments);
        }

        // GET: /Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = DepartRepos.FindById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: /Department/Create
        public ActionResult Create()
        {
            IEnumerable<Instructor> instructors = InstructorRepos.FindAll();
            ViewBag.InstructorID = new SelectList(instructors, "ID", "LastName");
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                //db.Departments.Add(department);
                //await db.SaveChangesAsync();
                DepartRepos.Create(department);
                return RedirectToAction("Index");
            }
            var instructors = InstructorRepos.FindAll();
            ViewBag.InstructorID = new SelectList(instructors, "ID", "LastName", department.InstructorID);
            return View(department);
        }

        // GET: /Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = DepartRepos.FindById(id);
            if (department == null) {
                return HttpNotFound();
            }
            IEnumerable<Instructor> instructors = InstructorRepos.FindAll();
            ViewBag.InstructorID = new SelectList(instructors, "ID", "LastName", department.InstructorID);
            return View(department);
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                DepartRepos.Update(department);
                return RedirectToAction("Index");
            }
            var instructors = InstructorRepos.FindAll();
            ViewBag.InstructorID = new SelectList(instructors, "ID", "LastName", department.InstructorID);
            return View(department);
        }

        // GET: /Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = DepartRepos.FindById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepartRepos.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DepartRepos.Dispose();
                InstructorRepos.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
