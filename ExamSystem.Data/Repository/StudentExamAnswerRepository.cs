using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Data.Repository
{
    public class StudentExamAnswerRepository : BaseRepository<StudentExamAnswer>, IStudentExamAnswerRepository
    {
        public StudentExamAnswerRepository(AppDBContext context) : base(context)
        {
        }
    }
}
