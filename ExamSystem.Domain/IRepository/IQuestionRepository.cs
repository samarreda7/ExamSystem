using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IQuestionRepository
    {
        Task AddAsync(Question entity);
        Task<IEnumerable<Question>> GetAllAsync();

        Task<Question?> GetByIdAsync(Guid id);

        Task UpdateAsync(Question entity);

        Task DeleteAsync(Guid id);
    }
}
