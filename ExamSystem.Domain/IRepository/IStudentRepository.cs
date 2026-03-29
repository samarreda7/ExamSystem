using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.IRepository
{
    public interface IStudentRepository
    {
        Task AddAsync(Student entity);
        Task<IEnumerable<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(Guid id);

        Task UpdateAsync(Student entity);

        Task DeleteAsync(Guid id);
    }
}
