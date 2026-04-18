using ExamSystem.Domain.Models;

namespace ExamSystem.Domain.IRepository
{
    public interface IStudentExamResultRepository : IBaseRepository<StudentExamResult>
    {
        Task<StudentExamResult?> GetByStudentIdAndExamIdAsync(Guid studentId, Guid examId);
    }
}
