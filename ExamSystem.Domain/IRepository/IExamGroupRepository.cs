using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IExamGroupRepository : IBaseRepository<ExamGroup>
    {
       
        Task<ExamGroup?> GetByIdAsync(Guid examId, Guid groupId);
        Task<IEnumerable<ExamGroup>> GetByExamAsync(Guid examId);
        Task<IEnumerable<ExamGroup>> GetByGroupAsync(Guid groupId);
 
    }
}
