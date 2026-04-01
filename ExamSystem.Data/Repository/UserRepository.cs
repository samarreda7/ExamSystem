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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDBContext context) : base(context) { }


        public async Task<bool> IsUsernameExist(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Username == username) != null;
        }
        public async Task<bool> IsEmailExist(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
        public async Task<bool> IsEmailExistForAnotherUser(string email, Guid excludeUserId)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.Id != excludeUserId);
        }

        public async Task<bool> IsUsernameExistForAnotherUser(string username, Guid excludeUserId)
        {
            return await _dbSet.AnyAsync(u => u.Username == username && u.Id != excludeUserId);
        }
    }
}
