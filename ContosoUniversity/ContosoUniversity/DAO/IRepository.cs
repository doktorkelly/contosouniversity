using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.DAO
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> FindAll();
        T FindById(int? id);
        void Create(T entity);
        void Update(T entity);
        void Remove(int id);
    }
}
