using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IStudentExamResultService
    {
        Task<ShowStudentExamResultDto> GetStudentResultByStudentIdAndExamIdAsync(Guid studentId, Guid examId);
    }
}
