using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;
using ContosoUniversity.DAC;
using System.Data.Entity;

namespace ContosoUniversity.DAO
{
    public class EFCourseRepository : ICourseRepository
    {
        private SchoolContext db;

        public EFCourseRepository(SchoolContext context)
        {
            db = context;
        }

        public void Create(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
        }

        public Course FindById(int? id)
        {
            return db.Courses.Find(id);
        }

        public IEnumerable<Course> FindAll()
        {
            return db.Courses;
        }

        public void Update(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}