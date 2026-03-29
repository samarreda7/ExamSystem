using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface ISubjectRepository
    {
        Task AddAsync(Subject entity);
        Task<IEnumerable<Subject>> GetAllAsync();

        Task<Subject?> GetByIdAsync(Guid id);

        Task UpdateAsync(Subject entity);

        Task DeleteAsync(Guid id);
    }
}
