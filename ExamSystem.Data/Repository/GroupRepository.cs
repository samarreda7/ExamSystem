using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class GroupRepository  : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(AppDBContext context) : base(context) { }



        public async Task<bool> IsGroupExistAsync(Guid groupId)
        {
            return await _dbSet.AnyAsync(x => x.Id == groupId);
        }
        public async Task<bool> IsGroupNameExistAsync(string groupName)
        {
            return await _dbSet.AnyAsync(x => x.Name == groupName);
        }
        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _dbSet.Include(g => g.Subject)
                               .Include(g => g.Teacher)
                               .ToListAsync();
             
        }
    }
}
