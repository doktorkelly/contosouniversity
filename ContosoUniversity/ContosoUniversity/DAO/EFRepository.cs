using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;
using ContosoUniversity.DAC;
using System.Data.Entity;

namespace ContosoUniversity.DAO
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private SchoolContext db;

        public EFRepository(SchoolContext context)
        {
            db = context;
        }

        public IEnumerable<T> FindAll()
        {
            return db.Set<T>();
        }

        public T FindById(int? id)
        {
            return db.Set<T>().Find(id);
        }

        public void Create(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();
        }

        public void Update(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            T entity = db.Set<T>().Find(id);
            db.Set<T>().Remove(entity);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}