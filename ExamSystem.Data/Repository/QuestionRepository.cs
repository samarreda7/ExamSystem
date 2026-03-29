using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DbSet<Question> _dbSet;

        public QuestionRepository(AppDBContext context)
        {
            _dbSet = context.Set<Question>();
        }

        public Task AddAsync(Question entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid id)
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

        public Task UpdateAsync(Question entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
    }
}
