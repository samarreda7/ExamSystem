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
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == groupId) != null;
        }
    }
}
