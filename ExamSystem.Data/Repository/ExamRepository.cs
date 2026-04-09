using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
         public ExamRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Exam>> GetAllTeacherExamAsync(Guid teacherId)
        {
            return await _dbSet.Where(e=>e.TeacherUserId== teacherId)
                               .Include(e=>e.Teacher)
                               .ThenInclude(t=>t.User)
                               .Include(e=>e.ExamGroups)
                               .Include(e=>e.ExamQuestions)
                               .ToListAsync();
        }



    }
}
