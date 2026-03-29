using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IGroupRepository
    {
        Task AddAsync(Group entity);
        Task<IEnumerable<Group>> GetAllAsync();

        Task<Group?> GetByIdAsync(Guid id);

        Task UpdateAsync(Group entity);

        Task DeleteAsync(Guid id);
    }
}
