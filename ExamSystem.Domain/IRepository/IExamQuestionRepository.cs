using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IExamQuestionRepository : IBaseRepository<ExamQuestion>
    {

        Task<ExamQuestion?> GetByIdAsync(Guid examId, Guid questionId);
        Task<IEnumerable<ExamQuestion>> GetByExamAsync(Guid examId);
        Task<IEnumerable<ExamQuestion>> GetByQuestionAsync(Guid questionId);


    }
}
