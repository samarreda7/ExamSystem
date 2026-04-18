using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class ExamQuestionRepository : BaseRepository<ExamQuestion>, IExamQuestionRepository
    {
        public ExamQuestionRepository(AppDBContext context) : base(context) { }


        public async Task<ExamQuestion?> GetByIdAsync(Guid examId, Guid questionId)
        {
            return await _dbSet.FindAsync(examId, questionId);
        }
        public async Task<IEnumerable<ExamQuestion>> GetByExamAsync(Guid examId)
        {
            return await _dbSet.Where(e => e.ExamId == examId)
                .Include(e => e.Question)
                .ThenInclude(q => q.Options)
                .ToListAsync();
        }
        public async Task<IEnumerable<ExamQuestion>> GetByQuestionAsync(Guid questionId)
        {
            return await _dbSet.Where(e => e.QuestionId == questionId)
                   .Include(e => e.Exam)
                   .ToListAsync();
        }

        public async Task<int> CountByExamIdAsync(Guid examId)
        {
            return await _dbSet.CountAsync(e => e.ExamId == examId);
        }

        public async Task<HashSet<Guid>> GetQuestionIdsByExamIdAsync(Guid examId)
        {
            return (await _dbSet.Where(e => e.ExamId == examId)
                .Select(e => e.QuestionId)
                .ToListAsync())
                .ToHashSet();
        }

    }
}
