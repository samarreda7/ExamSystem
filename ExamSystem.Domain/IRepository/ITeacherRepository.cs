using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface ITeacherRepository
    {
        Task AddAsync(Teacher entity);
        Task<IEnumerable<Teacher>> GetAllAsync();

        Task<Teacher?> GetByIdAsync(Guid id);

        Task UpdateAsync(Teacher entity);

        Task DeleteAsync(Guid id);
    }
}
