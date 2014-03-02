using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAO
{
    public interface ICourseRepository : IDisposable
    {
        void Create(Course course);
        void Delete(int? id);
        Course FindById(int? id);
        IEnumerable<Course> FindAll();
        void Update(Course course);
    }
}
