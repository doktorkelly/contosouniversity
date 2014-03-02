using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAO
{
    public interface IDepartmentRepository
    {
        void Create(Department entity);
        void Delete(int? id);
        Department FindById(int? id);
        IEnumerable<Department> FindAll();
        void Update(Department entity);
    }
}
