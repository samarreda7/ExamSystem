using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamService
    {
        Task AddExamAsync(Guid teacherId, CreateExamDto dto);
    }
}
