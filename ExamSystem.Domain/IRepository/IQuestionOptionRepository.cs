using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IQuestionOptionRepository : IBaseRepository<QuestionOption>
    {
        Task<int> CountByQuestionIdAsync(Guid questionId);
        Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId);
        Task<bool> HasCorrectOptionAsync(Guid questionId);
        Task<bool> HasOtherCorrectOptionAsync(Guid questionId, Guid optionId);
        Task<QuestionOption?> GetByIdAndQuestionIdAsync(Guid optionId, Guid questionId);
    }
}
