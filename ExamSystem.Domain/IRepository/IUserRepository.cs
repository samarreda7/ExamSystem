using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> IsUsernameExist(string username);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsEmailExistForAnotherUser(string email, Guid excludeUserId);
        Task<bool> IsUsernameExistForAnotherUser(string username, Guid excludeUserId);
        Task<User?> GetByEmailAsync(string email);


    }
}
