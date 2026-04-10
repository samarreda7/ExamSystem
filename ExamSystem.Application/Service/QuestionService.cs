using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;


namespace ExamSystem.Application.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitofwork;
        public QuestionService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task AddQuestionAsync(Guid teacherId, CreateQuestionDto dto)
        {
            if (dto == null) 
            {
            throw new ArgumentNullException(nameof(dto));
            }
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(dto.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"There is no subject with Id: {dto.SubjectId}");
            }
            if (teacher.SubjectId != dto.SubjectId)
            {
                throw new InvalidDataException("You can only add questions for your own subject.");
            }
            var question = new Question
            {
                Text = dto.Text,
                Type = dto.Type,
                SubjectId = dto.SubjectId,
                TeacherUserId = teacherId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _unitofwork.Questions.AddAsync(question);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
