using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class StudentGroupRepository : BaseRepository<StudentGroup> , IStudentGroupRepository
    {
        public StudentGroupRepository(AppDBContext context) : base(context) { }

        public Task<bool> IsStudentAssignedToThisGroupAsync(Guid StudentId, Guid GroupId)
        {
            return _dbSet.AnyAsync(s=>s.StudentId == StudentId && s.GroupId == GroupId);
        }
        public async Task<IEnumerable<StudentGroup>> GetGroupsByStudentIdAsync(Guid studentId)
        {
            return await _dbSet.Where(s => s.StudentId == studentId)
                .Include(s => s.Group)
                .ThenInclude(g => g.Subject)
                .Include(s => s.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public async Task<int> GetGroupCountByStudentIdAsync(Guid studentId)
        {
            return await _dbSet.CountAsync(s => s.StudentId == studentId);
        }

        public async Task<IEnumerable<StudentGroup>> GetStudentsByGroupIdAsync(Guid GroupId) 
        {
        return await _dbSet.Where(s=>s.GroupId == GroupId)
                .Include(s=>s.Student)
                .Include(s=>s.Student.User)
                .ToListAsync();
        }

        public async Task<int> GetStudentCountByGroupIdAsync(Guid groupId)
        {
            return await _dbSet.CountAsync(s => s.GroupId == groupId);
        }

        public async Task<StudentGroup?> GetStudentGroupAssign(Guid StudentId, Guid GroupId)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.StudentId == StudentId && s.GroupId == GroupId);
        }
    }
}
