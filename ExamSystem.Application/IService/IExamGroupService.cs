using ExamSystem.Application.DTO;

namespace ExamSystem.Application.IService
{
    public interface IExamGroupService
    {
        Task AssignExamToGroupAsync(Guid teacherId, AssignExamToGroupDto dto);
        Task<IEnumerable<ShowStudentAvailableExamDto>> GetExamsByGroupIdAsync(Guid studentId, Guid groupId);
        Task<IEnumerable<ShowStudentAvailableExamDto>> GetAvailableExamsByStudentIdAsync(Guid studentId);
        Task<int> GetExamCountByGroupIdAsync(Guid teacherId, Guid groupId);
        Task<int> GetAssignedExamCountByStudentIdAsync(Guid studentId);
        Task<bool> IsExamAssignedToGroupAsync(Guid teacherId, Guid examId, Guid groupId);
        Task RemoveExamFromGroupAsync(Guid teacherId, Guid examId, Guid groupId);
    }
}
