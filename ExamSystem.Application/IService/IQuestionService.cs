using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IQuestionService
    {
        Task AddQuestionAsync(Guid teacherId, CreateQuestionDto dto);
    }
}
