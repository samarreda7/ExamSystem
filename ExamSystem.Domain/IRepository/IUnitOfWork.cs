
using ExamSystem.Domain.Models;

namespace ExamSystem.Domain.IRepository;
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IExamRepository Exams { get; }
    IGroupRepository Groups { get; }
    IStudentRepository Students { get; }
    ITeacherRepository Teachers { get; }
    ISubjectRepository Subjects { get; }
    IQuestionRepository Questions { get; }
    IQuestionOptionRepository QuestionOptions { get; }
    IExamQuestionRepository ExamQuestions { get; }
    IExamGroupRepository ExamGroups { get; }
    IStudentGroupRepository StudentGroup {  get; }





    Task<int> SaveChangesAsync();
}
