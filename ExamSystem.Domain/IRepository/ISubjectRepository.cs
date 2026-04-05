using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {
        Task<bool> IsSubjectExistAsync(Guid Id);
        Task<bool> IsSubjectNameExistAsync(string subjectName);

    }
}
