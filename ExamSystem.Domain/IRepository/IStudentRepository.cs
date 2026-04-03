using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {

        Task<IEnumerable<Student>> GetAllWithDetailsAsync();
        Task<Student?> GetStudentDetailsById(Guid Id);


    }
}
