using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class StudentExamResultRepository : BaseRepository<StudentExamResult>, IStudentExamResultRepository
    {
        public StudentExamResultRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<StudentExamResult?> GetByStudentIdAndExamIdAsync(Guid studentId, Guid examId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.StudentId == studentId && x.ExamId == examId);
        }
    }
}
