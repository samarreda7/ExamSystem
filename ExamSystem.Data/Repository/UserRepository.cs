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
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDBContext context)
        {
            _dbSet = context.Set<User>();
        }

        public Task AddAsync(User entity)
        {
             _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();   
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async  Task DeleteAsync(Guid id)
        {
            var entity = await  GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);

            }
        }

        public Task UpdateAsync(User entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
        public async Task<bool> IsUsernameExist(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Username == username) != null;
        }
        public async Task<bool> IsEmailExist(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
    }
}
