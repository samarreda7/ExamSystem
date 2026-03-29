using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IExamRepository
    {
        Task AddAsync(Exam entity);
        Task<IEnumerable<Exam>> GetAllAsync();

        Task<Exam?> GetByIdAsync(Guid id);

        Task UpdateAsync(Exam entity);

        Task DeleteAsync(Guid id);

    }
}
