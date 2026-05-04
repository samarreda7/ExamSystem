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
        Task<ShowGroupDto> GetGroupByIdAsync(Guid groupId);
        Task<int> GetGroupsCountByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ShowGroupDto>> GetTeacherGroupsAsync(Guid teacherId);
    }
}
