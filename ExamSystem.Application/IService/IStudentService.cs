using ExamSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface IStudentService
    {
        Task AddStudentAsync(CreateStudentDto user);
        Task<IEnumerable<ShowStudentDto>> GetStudentsWithAllDetailsAsync();
        Task<ShowStudentDto> GetStudentByIdAsync(Guid id);
        Task UpdateStudentAsync(Guid id, UpdateStudentDto dto);
        Task DeleteStudentAsync(Guid id);
        Task<IEnumerable<ShowStudentDto>> GetStudentsByGroupIdAsync(Guid groupId);
    }
}
 