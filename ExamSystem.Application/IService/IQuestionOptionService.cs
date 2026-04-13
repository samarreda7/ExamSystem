using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IQuestionOptionService
    {
        Task AssignOptionToQuestionAsync(Guid teacherId, CreateQuestionOptionDto dto);
        Task DeleteOptionAsync(Guid optionId, Guid teacherId);
        Task<ShowQuestionOptionDto> GetOptionByIdAsync(Guid optionId, Guid teacherId);
        Task UpdateOptionAsync(Guid optionId, Guid teacherId, UpdateQuestionOptionDto dto);
    }
}
