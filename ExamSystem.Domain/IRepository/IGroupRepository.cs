using ExamSystem.Domain.Models;


namespace ExamSystem.Domain.IRepository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
  
        Task<bool> IsGroupExistAsync(Guid groupId);
        Task<bool> IsGroupOwnedByTeacherAsync(Guid groupId, Guid teacherId);
        Task<bool> IsGroupNameExistAsync(string groupName);
        Task<int> GetGroupsCountByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<Group?> GetGroupByIdWithDetailsAsync(Guid groupId);
        Task<IEnumerable<Group>> GetGroupsByTeacherIdAsync(Guid teacherId);
    }
}
