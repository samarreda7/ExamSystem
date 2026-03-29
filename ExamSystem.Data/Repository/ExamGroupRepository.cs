using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class ExamGroupRepository : IExamGroupRepository

    {
        private readonly DbSet<ExamGroup> _dbSet;

        public ExamGroupRepository(AppDBContext context)
        {
            _dbSet = context.Set<ExamGroup>();
        }

        public Task AddAsync(ExamGroup entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<ExamGroup>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<ExamGroup?> GetByIdAsync(Guid examId, Guid grouppId)
        {
            return await _dbSet.FindAsync(examId ,grouppId);
        }
        public async Task<IEnumerable<ExamGroup>> GetByExamAsync(Guid examId) { 
            return await _dbSet.Where(e=>e.ExamId == examId)
                .Include(e=>e.Group)
                .ToListAsync();
        }
        public async Task<IEnumerable<ExamGroup>> GetByGroupAsync(Guid groupId)
        {
            return await _dbSet.Where(e => e.GroupId == groupId)
                   .Include(e => e.Exam)
                   .ToListAsync();
        }


        public async Task DeleteAsync(Guid examId, Guid groupId)
        {
            var entity = await GetByIdAsync(examId, groupId);
            if (entity != null)
            {
                _dbSet.Remove(entity);

            }
        }

        public Task UpdateAsync(ExamGroup entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
    }
}
