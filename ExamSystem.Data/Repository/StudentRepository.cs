

using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbSet<Student> _dbSet;

        public StudentRepository(AppDBContext context)
        {
            _dbSet = context.Set<Student>();
        }

        public Task AddAsync(Student entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
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

        public Task UpdateAsync(Student entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;



        }
    }
}
