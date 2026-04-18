using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IStudentExamSubmissionService
    {
        Task SubmitExamAsync(Guid studentId, SubmitExamDto dto);
    }
}
