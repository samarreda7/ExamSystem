using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IStudentExamResultService
    {
        Task<ShowStudentExamResultDto> GetStudentResultByStudentIdAndExamIdAsync(Guid studentId, Guid examId);
        Task<IEnumerable<ShowStudentExamScoreDto>> GetStudentScoresByExamIdAsync(Guid teacherId, Guid examId);
    }
}
