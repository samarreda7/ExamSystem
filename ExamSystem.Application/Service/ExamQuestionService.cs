using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Application.Service
{
    public class ExamQuestionService : IExamQuestionService
    {
        private readonly IUnitOfWork _unitofwork;

        public ExamQuestionService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AssignQuestionToExamAsync(Guid teacherId, AssignQuestionToExamDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(dto.ExamId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {dto.ExamId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only assign questions to your own exams.");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(dto.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {dto.QuestionId}");
            }

            var examQuestion = await _unitofwork.ExamQuestions.GetByIdAsync(dto.ExamId, dto.QuestionId);
            if (examQuestion != null)
            {
                throw new InvalidOperationException("This question is already assigned to this exam.");
            }

            var newExamQuestion = new ExamQuestion
            {
                ExamId = dto.ExamId,
                QuestionId = dto.QuestionId
            };

            await _unitofwork.ExamQuestions.AddAsync(newExamQuestion);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
