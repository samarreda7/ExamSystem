using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Data.Repository
{
    public class StudentExamResultRepository : BaseRepository<StudentExamResult>, IStudentExamResultRepository
    {
        public StudentExamResultRepository(AppDBContext context) : base(context)
        {
        }
    }
}
