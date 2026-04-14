using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamGroupService
    {
        Task AssignExamToGroupAsync(Guid teacherId, AssignExamToGroupDto dto);
        Task RemoveExamFromGroupAsync(Guid teacherId, Guid examId, Guid groupId);
    }
}
