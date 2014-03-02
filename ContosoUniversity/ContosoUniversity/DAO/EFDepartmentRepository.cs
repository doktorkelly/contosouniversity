using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;
using ContosoUniversity.DAC;

namespace ContosoUniversity.DAO
{
    public class EFDepartmentRepository : IDepartmentRepository
    {
        private SchoolContext db;

        public EFDepartmentRepository(SchoolContext context)
        {
            db = context;
        }

        public void Create(Department entity)
        {
            db.Departments.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Department entity = db.Departments.Find(id);
            db.Departments.Remove(entity);
            db.SaveChanges();
        }

        public Department FindById(int? id)
        {
            return db.Departments.Find(id);
        }

        public IEnumerable<Department> FindAll()
        {
            return db.Departments;
        }

        public void Update(Department entity)
        {
            db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}