using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;


namespace ExamSystem.Application.Service
{
    public class ExamService :IExamService
    {
        private readonly IUnitOfWork _unitofwork;
        public ExamService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task AddExamAsync(Guid teacherId,CreateExamDto dto)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var exam = new Exam
            {
                Name = dto.Name,
                TeacherUserId = teacher.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _unitofwork.Exams.AddAsync(exam);
            await _unitofwork.SaveChangesAsync();
        }
        public async Task<IEnumerable<ShowExamDto>> GetTeacherExamsAsync(Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var exams = await _unitofwork.Exams.GetAllTeacherExamAsync(teacherId);
            return exams.Select(exam => new ShowExamDto
            {
                Id = exam.Id,
                Name = exam.Name,
                TeacherFirstName = exam.Teacher.User.FirstName,
                TeacherLastName = exam.Teacher.User.LastName,
                QuestionsCount = exam.ExamQuestions.Count,
                GroupsCount = exam.ExamGroups.Count,
            });
        }  
    }
}
