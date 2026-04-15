using ExamSystem.Domain.Models;


namespace ExamSystem.Domain.IRepository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
  
        Task<bool> IsGroupExistAsync(Guid groupId);
        Task<bool> IsGroupNameExistAsync(string groupName);
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<IEnumerable<Group>> GetGroupsByTeacherIdAsync(Guid teacherId);
    }
}
