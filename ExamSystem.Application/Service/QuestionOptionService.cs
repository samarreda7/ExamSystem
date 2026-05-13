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

        public async Task DeleteOptionAsync(Guid optionId, Guid teacherId)
        {
            var option = await _unitofwork.QuestionOptions.GetByIdAsync(optionId);
            if (option == null)
            {
                throw new KeyNotFoundException($"There is no option with Id: {optionId}");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(option.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {option.QuestionId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only delete options for your own questions.");
            }

            if (option.IsCorrect)
            {
                var examQuestions = await _unitofwork.ExamQuestions.GetByQuestionAsync(question.Id);
                foreach (var examQuestion in examQuestions)
                {
                    await _unitofwork.ExamQuestions.DeleteAsync(examQuestion);
                }
            }

            await _unitofwork.QuestionOptions.DeleteAsync(option);
            await _unitofwork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShowQuestionOptionDto>> GetOptionsByQuestionIdAsync(Guid questionId, Guid teacherId)
        {
            var question = await _unitofwork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {questionId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (teacher.SubjectId != question.SubjectId)
            {
                throw new UnauthorizedAccessException("You can only get options for questions in your own subject.");
            }

            var options = await _unitofwork.QuestionOptions.GetByQuestionIdAsync(questionId);

            return options.Select(option => new ShowQuestionOptionDto
            {
                Id = option.Id,
                Text = option.Text,
                IsCorrect = option.IsCorrect,
                QuestionId = option.QuestionId
            });
        }

        public async Task<ShowQuestionOptionDto> GetOptionByIdAsync(Guid optionId, Guid teacherId)
        {
            var option = await _unitofwork.QuestionOptions.GetByIdAsync(optionId);
            if (option == null)
            {
                throw new KeyNotFoundException($"There is no option with Id: {optionId}");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(option.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {option.QuestionId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only get options for your own questions.");
            }

            return new ShowQuestionOptionDto
            {
                Id = option.Id,
                Text = option.Text,
                IsCorrect = option.IsCorrect,
                QuestionId = option.QuestionId
            };
        }

        public async Task UpdateOptionAsync(Guid optionId, Guid teacherId, UpdateQuestionOptionDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Text))
            {
                throw new InvalidDataException("Option text can not be empty.");
            }

            var option = await _unitofwork.QuestionOptions.GetByIdAsync(optionId);
            if (option == null)
            {
                throw new KeyNotFoundException($"There is no option with Id: {optionId}");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(option.QuestionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {option.QuestionId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only update options for your own questions.");
            }

            if (dto.IsCorrect)
            {
                var hasOtherCorrectOption = await _unitofwork.QuestionOptions
                    .HasOtherCorrectOptionAsync(question.Id, optionId);
                if (hasOtherCorrectOption)
                {
                    throw new InvalidOperationException("This question already has a correct option.");
                }
            }

            option.Text = dto.Text;
            option.IsCorrect = dto.IsCorrect;

            await _unitofwork.QuestionOptions.UpdateAsync(option);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
