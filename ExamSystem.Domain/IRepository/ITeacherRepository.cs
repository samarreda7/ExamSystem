using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        Task<IEnumerable<Teacher>> GetAllWithDetailsAsync();
        Task<Teacher?> GetTeacherDetailsById(Guid Id);
    }
}
