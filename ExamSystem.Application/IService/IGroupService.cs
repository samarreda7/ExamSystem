using ExamSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface IGroupService
    {
        Task<ShowGroupDto> AddGroupAsync(Guid teacherId, CreateGroupDto groupDto);
        Task<IEnumerable<ShowGroupDto>> GetAllGroupsAsync();
        Task<IEnumerable<ShowGroupDto>> GetTeacherGroupsAsync(Guid teacherId);
    }
}
