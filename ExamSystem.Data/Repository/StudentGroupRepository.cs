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
    }
}
