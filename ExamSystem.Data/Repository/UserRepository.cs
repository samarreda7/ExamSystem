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
    }
}
