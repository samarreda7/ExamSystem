using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
         public ExamRepository(AppDBContext context) : base(context) { }





    }
}
