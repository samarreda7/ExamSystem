

using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDBContext context) : base(context) { }



        public async Task<IEnumerable<Student>> GetAllWithDetailsAsync()
        {
            return await _dbSet.Include(s => s.User)
                .ToListAsync();
        }
        public async Task<Student?> GetStudentDetailsById(Guid Id)
        {
            return await _dbSet.Where(s => s.UserId == Id)
                .Include(s => s.User)
                .FirstOrDefaultAsync();
        }

    }
}
