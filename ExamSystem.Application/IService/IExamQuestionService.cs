using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamQuestionService
    {
        Task AssignQuestionToExamAsync(Guid teacherId, AssignQuestionToExamDto dto);
        Task<IEnumerable<ShowQuestionByExamIdDto>> GetQuestionsByExamIdAsync(Guid teacherId, Guid examId);
        Task RemoveQuestionFromExamAsync(Guid teacherId, Guid examId, Guid questionId);
    }
}
