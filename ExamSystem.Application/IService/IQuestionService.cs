using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IQuestionService
    {
        Task AddQuestionAsync(Guid teacherId, CreateQuestionDto dto);
        Task DeleteQuestionAsync(Guid questionId, Guid teacherId);
        Task<IEnumerable<string>> GetQuestionTypesAsync();
        Task<ShowQuestionDto> GetQuestionByIdAsync(Guid questionId, Guid teacherId);
        Task<IEnumerable<ShowQuestionDto>> GetQuestionsBySubjectAsync(Guid subjectId, Guid teacherId);
        Task UpdateQuestionAsync(Guid questionId, Guid teacherId, UpdateQuestionDto dto);
    }
}
    
