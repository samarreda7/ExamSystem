using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class ExamGroupRepository : BaseRepository<ExamGroup>, IExamGroupRepository

    {
        public ExamGroupRepository(AppDBContext context) : base(context) { }


        public async Task<ExamGroup?> GetByIdAsync(Guid examId, Guid grouppId)
        {
            return await _dbSet.FindAsync(examId ,grouppId);
        }
        public async Task<IEnumerable<ExamGroup>> GetByExamAsync(Guid examId) { 
            return await _dbSet.Where(e=>e.ExamId == examId)
                .Include(e=>e.Group)
                .ToListAsync();
        }
        public async Task<IEnumerable<ExamGroup>> GetByGroupAsync(Guid groupId)
        {
            return await _dbSet.Where(e => e.GroupId == groupId)
                   .Include(e => e.Exam)
                   .Include(e => e.Group)
                   .ThenInclude(g => g.Subject)
                   .ToListAsync();
        }

        public async Task<bool> IsExamAssignedToGroupAsync(Guid examId, Guid groupId)
        {
            return await _dbSet.AnyAsync(e => e.ExamId == examId && e.GroupId == groupId);
        }

        public async Task<bool> IsExamAssignedToAnyGroupAsync(Guid examId)
        {
            return await _dbSet.AnyAsync(e => e.ExamId == examId);
        }

        public async Task<bool> IsStudentAssignedToExamAsync(Guid studentId, Guid examId)
        {
            return await _context.student_group
                .Join(_dbSet,
                    studentGroup => studentGroup.GroupId,
                    examGroup => examGroup.GroupId,
                    (studentGroup, examGroup) => new { studentGroup, examGroup })
                .AnyAsync(x => x.studentGroup.StudentId == studentId && x.examGroup.ExamId == examId);
        }


  
    }
}
