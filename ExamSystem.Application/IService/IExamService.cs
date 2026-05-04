using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamService
    {
        Task AddExamAsync(Guid teacherId, CreateExamDto dto);
        Task<int> GetExamsCountByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ShowExamDto>> GetTeacherExamsAsync(Guid teacherId);
        Task<ShowExamDto> GetExamByIdAsync(Guid teacherId, Guid examId);
        Task UpdateExamAsync(Guid teacherId, Guid examId, UpdateExamDto dto);
        Task DeleteExamAsync(Guid teacherId, Guid examId);
    }
}
