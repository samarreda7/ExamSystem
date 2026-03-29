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
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DbSet<Teacher> _dbSet;

        public TeacherRepository(AppDBContext context)
        {
            _dbSet = context.Set<Teacher>();
        }

        public Task AddAsync(Teacher entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(Guid id)
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

        public Task UpdateAsync(Teacher entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;



        }
    }
}
