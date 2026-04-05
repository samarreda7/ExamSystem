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
        Task AddGroupAsync(CreateGroupDto groupDto);
        Task<IEnumerable<ShowGroupDto>> GetAllGroupsAsync();
    }
}
