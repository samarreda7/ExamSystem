using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IQuestionOptionRepository
    {
        Task AddAsync(QuestionOption entity);
        Task<IEnumerable<QuestionOption>> GetAllAsync();

        Task<QuestionOption?> GetByIdAsync(Guid id);

        Task UpdateAsync(QuestionOption entity);

        Task DeleteAsync(Guid id);
    }
}
