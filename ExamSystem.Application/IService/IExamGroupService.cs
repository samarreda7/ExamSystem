using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamGroupService
    {
        Task AssignExamToGroupAsync(Guid teacherId, AssignExamToGroupDto dto);
        Task<IEnumerable<ShowExamByGroupIdForStudentDto>> GetExamsByGroupIdAsync(Guid studentId, Guid groupId);
        Task<bool> IsExamAssignedToGroupAsync(Guid teacherId, Guid examId, Guid groupId);
        Task RemoveExamFromGroupAsync(Guid teacherId, Guid examId, Guid groupId);
    }
}
