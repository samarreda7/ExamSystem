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
        public async Task<bool> IsGroupOwnedByTeacherAsync(Guid groupId, Guid teacherId)
        {
            return await _dbSet.AnyAsync(x => x.Id == groupId && x.TeacherUserId == teacherId);
        }
        public async Task<bool> IsGroupNameExistAsync(string groupName)
        {
            return await _dbSet.AnyAsync(x => x.Name == groupName);
        }

        public async Task<int> GetGroupsCountByTeacherIdAsync(Guid teacherId)
        {
            return await _dbSet.CountAsync(g => g.TeacherUserId == teacherId);
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _dbSet.Include(g => g.Subject)
                               .Include(g => g.Teacher)
                               .ToListAsync();
             
        }

        public async Task<Group?> GetGroupByIdWithDetailsAsync(Guid groupId)
        {
            return await _dbSet
                .Include(g => g.Subject)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task<IEnumerable<Group>> GetGroupsByTeacherIdAsync(Guid teacherId)
        {
            return await _dbSet.Include(g => g.Subject)
                               .Include(g => g.Teacher)
                               .Where(g => g.TeacherUserId == teacherId)
                               .ToListAsync();
        }
    }
}
