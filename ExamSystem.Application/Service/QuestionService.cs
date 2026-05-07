using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;


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
        public async Task DeleteQuestionAsync(Guid questionId, Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var question = await _unitofwork.Questions.GetByIdAsync(questionId);
            if(question == null)
            {
                throw new KeyNotFoundException("there is no question with this Id");
            }
            if(question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only delete questions you own");
            }
            await _unitofwork.Questions.DeleteAsync(question);
            await _unitofwork.SaveChangesAsync();
        }
        public Task<IEnumerable<string>> GetQuestionTypesAsync()
        {
            IEnumerable<string> questionTypes = Enum.GetNames<QuestionType>();
            return Task.FromResult(questionTypes);
        }
        public async Task<ShowQuestionDto> GetQuestionByIdAsync(Guid questionId, Guid teacherId)
        {
            var question = await _unitofwork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException("there is no question with this Id");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (teacher.SubjectId != question.SubjectId)
            {
                throw new InvalidDataException("You can only get questions for your own subject.");
            }

            var questionTeacher = await _unitofwork.Teachers.GetByIdAsync(question.TeacherUserId);
            var teacherFirstName = questionTeacher?.User?.FirstName ?? string.Empty;
            var teacherLastName = questionTeacher?.User?.LastName ?? string.Empty;

            return new ShowQuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Type = question.Type,
                TeacherFirstName = teacherFirstName,
                TeacherLastName = teacherLastName,
            };
        }
        public async Task<IEnumerable<ShowQuestionDto>> GetQuestionsBySubjectAsync(Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(teacher.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"There is no subject with Id: {teacher.SubjectId}");
            }

            var question = await _unitofwork.Questions.GetQuestionsBySubjectIdAsync(teacher.SubjectId);
            return question.Select(x => new ShowQuestionDto
            {
                Id = x.Id,
                Text = x.Text,
                Type = x.Type,
                TeacherFirstName = x.Teacher.User.FirstName,
                TeacherLastName = x.Teacher.User.LastName,
            });
        }
        public async Task UpdateQuestionAsync(Guid questionId, Guid teacherId, UpdateQuestionDto dto)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var question = await _unitofwork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException("there is no question with this Id");
            }
            if (question.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only update questions you own");
            }
            if(dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            if (dto.Text == null) 
            {
                throw new InvalidDataException("Question text can not be null");
            }
            question.Text = dto.Text;
            await _unitofwork.Questions.UpdateAsync(question);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
