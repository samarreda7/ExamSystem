using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _Context;
        public IUserRepository Users { get; }
        public IExamRepository Exams { get; }
        public IGroupRepository Groups { get; }
        public IStudentRepository Students { get; }
        public ITeacherRepository Teachers { get; }
        public ISubjectRepository Subjects { get; }
        public IQuestionRepository Questions { get; }
        public IQuestionOptionRepository QuestionOptions { get; }
        public IExamQuestionRepository ExamQuestions { get; }
        public IExamGroupRepository ExamGroups { get; }
        public IStudentGroupRepository StudentGroup { get; }
        public IRoleRepository Roles { get; }





        public UnitOfWork(AppDBContext context)
        {
            _Context = context;
            Users = new UserRepository(context);
            Exams = new ExamRepository(context);
            Groups = new GroupRepository(context);
            Students = new StudentRepository(context);
            Teachers = new TeacherRepository(context);
            Subjects = new SubjectRepository(context);
            Questions = new QuestionRepository(context);
            QuestionOptions = new QuestionOptionRepository(context);
            ExamQuestions = new ExamQuestionRepository(context);
            ExamGroups = new ExamGroupRepository(context);
            StudentGroup = new StudentGroupRepository(context);
            Roles = new RoleRepository(context);

        }

        public async Task<int> SaveChangesAsync() =>
                   await _Context.SaveChangesAsync();

        public void Dispose() => _Context.Dispose();
    }
}
