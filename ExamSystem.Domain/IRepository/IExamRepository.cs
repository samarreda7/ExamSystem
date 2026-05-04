using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IExamRepository : IBaseRepository<Exam>
    {
        Task<int> GetExamsCountByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Exam>> GetAllTeacherExamAsync(Guid teacherId);
        Task<Exam?> GetExamByIdWithDetailsAsync(Guid examId);
    }
}
