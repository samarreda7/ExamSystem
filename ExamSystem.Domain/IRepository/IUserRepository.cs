using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IUserRepository
    {
        Task AddAsync(User entity);
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync (Guid id);

        Task UpdateAsync(User entity);

        Task DeleteAsync(Guid id);

    }
}
