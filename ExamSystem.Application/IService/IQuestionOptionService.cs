using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IQuestionOptionService
    {
        Task AssignOptionToQuestionAsync(Guid teacherId, CreateQuestionOptionDto dto);
    }
}
