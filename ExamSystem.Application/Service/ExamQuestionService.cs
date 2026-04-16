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

        public async Task<IEnumerable<ShowQuestionByExamIdDto>> GetQuestionsByExamIdAsync(Guid teacherId, Guid examId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only get questions from your own exams.");
            }

            var examQuestions = await _unitofwork.ExamQuestions.GetByExamAsync(examId);

            return examQuestions.Select(eq => new ShowQuestionByExamIdDto
            {
                QuestionId = eq.Question.Id,
                Text = eq.Question.Text,
                Type = eq.Question.Type
            });
        }

        public async Task<IEnumerable<ShowQuestionWithOptionsByExamIdDto>> GetQuestionsByExamIdForStudentAsync(Guid studentId, Guid examId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }

            var examGroups = await _unitofwork.ExamGroups.GetByExamAsync(examId);
            if (!examGroups.Any())
            {
                throw new InvalidOperationException("This exam is not assigned to any group.");
            }

            var studentGroups = await _unitofwork.StudentGroup.GetGroupsByStudentIdAsync(studentId);
            var studentGroupIds = studentGroups.Select(sg => sg.GroupId).ToHashSet();

            bool isStudentAllowedToAccessExam = examGroups.Any(eg => studentGroupIds.Contains(eg.GroupId));
            if (!isStudentAllowedToAccessExam)
            {
                throw new UnauthorizedAccessException("This student is not assigned to a group that has this exam.");
            }

            var examQuestions = await _unitofwork.ExamQuestions.GetByExamAsync(examId);

            return examQuestions.Select(eq => new ShowQuestionWithOptionsByExamIdDto
            {
                QuestionId = eq.Question.Id,
                Text = eq.Question.Text,
                Type = eq.Question.Type,
                Options = eq.Question.Options.Select(option => new ShowQuestionOptionDto
                {
                    Id = option.Id,
                    Text = option.Text,
                    IsCorrect = option.IsCorrect,
                    QuestionId = option.QuestionId
                })
            });
        }

        public async Task RemoveQuestionFromExamAsync(Guid teacherId, Guid examId, Guid questionId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only remove questions from your own exams.");
            }

            var question = await _unitofwork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException($"There is no question with Id: {questionId}");
            }

            var examQuestion = await _unitofwork.ExamQuestions.GetByIdAsync(examId, questionId);
            if (examQuestion == null)
            {
                throw new InvalidOperationException("This question is not assigned to this exam.");
            }

            await _unitofwork.ExamQuestions.DeleteAsync(examQuestion);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
