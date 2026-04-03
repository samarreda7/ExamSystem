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
    }
}
