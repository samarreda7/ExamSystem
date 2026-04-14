using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamQuestionService
    {
        Task AssignQuestionToExamAsync(Guid teacherId, AssignQuestionToExamDto dto);
        Task RemoveQuestionFromExamAsync(Guid teacherId, Guid examId, Guid questionId);
    }
}
