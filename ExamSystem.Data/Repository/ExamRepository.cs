using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly DbSet<Exam> _dbSet;
        public ExamRepository(AppDBContext context)
        {
            _dbSet = context.Set<Exam>();
        }
        public Task AddAsync(Exam entity)
        {
             _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Exam?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public Task UpdateAsync(Exam entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
    }
}
