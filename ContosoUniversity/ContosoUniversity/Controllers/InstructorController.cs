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
using ContosoUniversity.ViewModels;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        //private SchoolContext db = new SchoolContext();
        IRepository<Instructor> InstructorRepos;
        IRepository<Course> CourseRepos;
        IRepository<Department> DepartmentRepos;

        public InstructorController()
        {
            SchoolContext db = new SchoolContext();
            InstructorRepos = new EFRepository<Instructor>(db);
            CourseRepos = new EFRepository<Course>(db);
            DepartmentRepos = new EFRepository<Department>(db);
        }

        public InstructorController(
            IRepository<Instructor> instructorRepos, 
            IRepository<Course> courseRepos,
            IRepository<Department> departmentRepos)
        {
            InstructorRepos = instructorRepos;
            CourseRepos = courseRepos;
            DepartmentRepos = departmentRepos;
        }

        // GET: /Instructor/
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = InstructorRepos.FindAll()
                //.Include(i => i.OfficeAssignment)
                //.Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if (id != null) {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors
                    .Single(i => i.ID == id.Value)
                    .Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                viewModel.Enrollments = viewModel.Courses
                    .Single(c => c.CourseID == courseID)
                    .Enrollments;
            }

            return View(viewModel);
        }

        // GET: /Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = InstructorRepos.FindById(id);
            if (instructor == null) {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: /Instructor/Create
        public ActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<Course>();
            ViewBag.Courses = AssignedCourses(instructor);
            return View();
        }

        // POST: /Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,HireDate")] Instructor instructor, FormCollection formCollection, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = CourseRepos.FindById(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                InstructorRepos.Create(instructor);
                return RedirectToAction("Index");
            }
            ViewBag.Courses = AssignedCourses(instructor);
            return View(instructor);
        }

        // GET: /Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = InstructorRepos.FindAll()
                //.Include(i => i.OfficeAssignment)
                //.Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();
            ViewBag.Courses = AssignedCourses(instructor);
            if (instructor == null) {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: /Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, FormCollection formCollection, string[] selectedCourses)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = InstructorRepos.FindAll()
                //.Include(i => i.OfficeAssignment)
                //.Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();
            if (TryUpdateModel(instructorToUpdate, "",
                new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" })) {
                try {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location)) {
                        instructorToUpdate.OfficeAssignment = null;
                    }
                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                    InstructorRepos.Update(instructorToUpdate);
                    //db.Entry<Instructor>(instructorToUpdate).State = EntityState.Modified;
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException dex) {
                    ModelState.AddModelError("", "unable to save changes");
                }
            }
            return View(instructorToUpdate);
        }


        // GET: /Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = InstructorRepos.FindById(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: /Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = InstructorRepos.FindAll()
                //.Include(i => i.OfficeAssignment)
                .Where(i => i.ID == id)
                .Single();

            instructor.OfficeAssignment = null;

            //db.Instructors.Remove(instructor);
            InstructorRepos.Remove(instructor.ID);

            var department = DepartmentRepos.FindAll()
                .Where(d => d.InstructorID == instructor.ID)
                .SingleOrDefault();

            if (department != null) {
                department.InstructorID = null;
            }
            DepartmentRepos.Update(department);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InstructorRepos.Dispose();
                CourseRepos.Dispose();
                DepartmentRepos.Dispose();
            }
            base.Dispose(disposing);
        }

        private IList<AssignedCourseData> AssignedCourses(Instructor instructor)
        {
            var viewModel = new List<AssignedCourseData>();
            var allCourse = CourseRepos.FindAll();
            var instructorCourseIds = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            foreach (var course in allCourse)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourseIds.Contains(course.CourseID)
                });
            }
            return viewModel;
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null) {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in CourseRepos.FindAll())
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString())) {
                    if (!instructorCourses.Contains(course.CourseID)) {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else {
                    if (instructorCourses.Contains(course.CourseID)) {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }


    }
}
