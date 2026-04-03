using ExamSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface IStudentGroupService
    {
        Task AssignStudentToGroupAsync(Guid studentId, Guid groupId, Guid teacherId);
        Task<IEnumerable<ShowStudentDto>> GetStudentsByGroupIdAsync(Guid groupId, Guid teacherId);
        Task ReassignStudentToAnotherGroupAsync(Guid groupId, Guid studentId, Guid NewGroupId, Guid teacherId);
        Task DeleteStudentAssignToGroupAsync(Guid studentId, Guid groupId, Guid teacherId);
    }
}
