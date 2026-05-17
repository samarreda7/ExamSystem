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
        Task<IEnumerable<ExamGroup>> GetAvailableExamsByStudentIdAsync(Guid studentId);
        Task<int> GetExamCountByGroupIdAsync(Guid groupId);
        Task<int> GetAssignedExamCountByStudentIdAsync(Guid studentId);
        Task<bool> IsExamAssignedToGroupAsync(Guid examId, Guid groupId);
        Task<bool> IsExamAssignedToAnyGroupAsync(Guid examId);
        Task<bool> IsStudentAssignedToExamAsync(Guid studentId, Guid examId);
 
    }
}
