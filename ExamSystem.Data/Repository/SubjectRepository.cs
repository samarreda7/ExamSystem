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
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DbSet<Subject> _dbSet;

        public SubjectRepository(AppDBContext context)
        {
            _dbSet = context.Set<Subject>();
        }

        public Task AddAsync(Subject entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(Guid id)
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

        public Task UpdateAsync(Subject entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }

    }
}
