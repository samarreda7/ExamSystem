using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;

namespace ExamSystem.Application.Service
{
    public class QuestionOptionService : IQuestionOptionService
    {
        private readonly IUnitOfWork _unitofwork;

        public QuestionOptionService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AssignOptionToQuestionAsync(Guid teacherId, CreateQuestionOptionDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Text))
            {
                throw new InvalidDataException("Option text can not be empty.");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(dto.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {dto.QuestionId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only add options to your own questions.");
            }

            if (dto.IsCorrect)
            {
                var hasCorrectOption = await _unitofwork.QuestionOptions.HasCorrectOptionAsync(dto.QuestionId);
                if (hasCorrectOption)
                {
                    throw new InvalidOperationException("This question already has a correct option.");
                }
            }

            if (question.Type == QuestionType.TF)
            {
                var optionCount = await _unitofwork.QuestionOptions.CountByQuestionIdAsync(dto.QuestionId);
                if (optionCount >= 2)
                {
                    throw new InvalidOperationException("True/False questions can have at most two options.");
                }
            }

            var option = new QuestionOption
            {
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
                QuestionId = dto.QuestionId
            };

            await _unitofwork.QuestionOptions.AddAsync(option);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
