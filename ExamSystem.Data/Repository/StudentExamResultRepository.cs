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

        public async Task<IEnumerable<StudentExamResult>> GetByExamIdAndStudentIdsAsync(Guid examId, IEnumerable<Guid> studentIds)
        {
            var studentIdList = studentIds.ToList();

            return await _dbSet
                .Where(x => x.ExamId == examId && studentIdList.Contains(x.StudentId))
                .ToListAsync();
        }

        public async Task<HashSet<Guid>> GetSubmittedExamIdsByStudentIdAsync(Guid studentId)
        {
            return await _dbSet
                .Where(x => x.StudentId == studentId)
                .Select(x => x.ExamId)
                .ToHashSetAsync();
        }
    }
}
