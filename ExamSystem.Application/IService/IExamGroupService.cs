using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamGroupService
    {
        Task AssignExamToGroupAsync(Guid teacherId, AssignExamToGroupDto dto);
    }
}
