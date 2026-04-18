using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IStudentExamResultService
    {
        Task SubmitExamAsync(Guid studentId, SubmitExamDto dto);
        Task<ShowStudentExamResultDto> GetStudentResultByStudentIdAndExamIdAsync(Guid studentId, Guid examId);
    }
}
