using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        private readonly DbSet<ExamQuestion> _dbSet;

        public ExamQuestionRepository(AppDBContext context)
        {
            _dbSet = context.Set<ExamQuestion>();
        }

        public Task AddAsync(ExamQuestion entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<ExamQuestion>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<ExamQuestion?> GetByIdAsync(Guid examId, Guid questionId)
        {
            return await _dbSet.FindAsync(examId, questionId);
        }
        public async Task<IEnumerable<ExamQuestion>> GetByExamAsync(Guid examId)
        {
            return await _dbSet.Where(e => e.ExamId == examId)
                .Include(e => e.Question)
                .ToListAsync();
        }
        public async Task<IEnumerable<ExamQuestion>> GetByQuestionAsync(Guid questionId)
        {
            return await _dbSet.Where(e => e.QuestionId == questionId)
                   .Include(e => e.Exam)
                   .ToListAsync();
        }

        public async Task DeleteAsync(Guid examId, Guid questionId)
        {
            var entity = await GetByIdAsync(examId,questionId);
            if (entity != null)
            {
                _dbSet.Remove(entity);

            }
        }

        public Task UpdateAsync(ExamQuestion entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
    }
}
