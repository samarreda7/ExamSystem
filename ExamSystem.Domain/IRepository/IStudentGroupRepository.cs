using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IStudentGroupRepository : IBaseRepository<StudentGroup>
    {
        Task<bool> IsStudentAssignedToThisGroupAsync(Guid StudentId, Guid GroupId);
        Task<IEnumerable<StudentGroup>> GetStudentsByGroupIdAsync(Guid GroupId);
        Task<StudentGroup?> GetStudentGroupAssign(Guid StudentId, Guid GroupId);
    }
}
