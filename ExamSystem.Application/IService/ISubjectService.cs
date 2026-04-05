using ExamSystem.Application.DTO;
using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface ISubjectService
    {
        Task AddSubjectAsync(CreateSubjectDto subjectDto);
        Task<IEnumerable<ShowSubjectsDto>> GetAllSubjectsAsync();
    }
}
