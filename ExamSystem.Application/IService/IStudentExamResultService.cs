using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IStudentExamResultService
    {
        Task<ShowStudentExamResultDto> GetStudentResultByStudentIdAndExamIdAsync(Guid studentId, Guid examId);
        Task<IEnumerable<ShowStudentExamScoreDto>> GetStudentScoresByGroupIdAndExamIdAsync(Guid teacherId, Guid groupId, Guid examId);
    }
}
