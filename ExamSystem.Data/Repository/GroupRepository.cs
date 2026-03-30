using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class GroupRepository  :IGroupRepository
    {
        private readonly DbSet<Group> _dbSet;
        public GroupRepository(AppDBContext context)
        {
            _dbSet = context.Set<Group>();
        }
        public Task AddAsync(Group entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;

        }
        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Group?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public Task UpdateAsync(Group entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
        public async Task<bool> IsGroupExistAsync(Guid groupId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == groupId) != null;
        }
    }
}
