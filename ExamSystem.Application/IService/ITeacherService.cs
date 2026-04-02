using ExamSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface ITeacherService
    {
        Task AddTeacherAsync(CreateTeacherDto teacherDto);
        Task<IEnumerable<ShowTeacherDto>> GetTeachersWithAllDetailsAsync();
        Task<ShowTeacherDto> GetTeacherByIdAsync(Guid id);
        Task DeleteTeacherAsync(Guid id);
        Task UpdateTeacherAsync(Guid id, UpdateTeacherDto dto);
        Task<IEnumerable<ShowTeacherDto>> GetTeachersBySubjectIdAsync(Guid subjectId);

    }
}
