using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Application.Service
{
    public class StudentExamSubmissionService : IStudentExamSubmissionService
    {
        private readonly IUnitOfWork _unitofwork;

        public StudentExamSubmissionService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task SubmitExamAsync(Guid studentId, SubmitExamDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Answers == null || !dto.Answers.Any())
            {
                throw new InvalidOperationException("You must answer all exam questions before submitting.");
            }

            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(dto.ExamId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {dto.ExamId}");
            }

            bool isExamAssignedToAnyGroup = await _unitofwork.ExamGroups.IsExamAssignedToAnyGroupAsync(dto.ExamId);
            if (!isExamAssignedToAnyGroup)
            {
                throw new InvalidOperationException("This exam is not assigned to any group.");
            }

            bool canAccessExam = await _unitofwork.ExamGroups.IsStudentAssignedToExamAsync(studentId, dto.ExamId);
            if (!canAccessExam)
            {
                throw new UnauthorizedAccessException("This student is not assigned to a group that has this exam.");
            }

            var existingResult = await _unitofwork.StudentExamResults.GetByStudentIdAndExamIdAsync(studentId, dto.ExamId);
            if (existingResult != null)
            {
                throw new InvalidOperationException("This student has already submitted this exam.");
            }

            int examQuestionsCount = await _unitofwork.ExamQuestions.CountByExamIdAsync(dto.ExamId);
            if (examQuestionsCount == 0)
            {
                throw new InvalidOperationException("This exam has no questions assigned.");
            }

            if (dto.Answers.Count != examQuestionsCount)
            {
                throw new InvalidOperationException("You must answer all exam questions before submitting.");
            }

            var duplicateQuestionIds = dto.Answers
                .GroupBy(x => x.QuestionId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateQuestionIds.Any())
            {
                throw new InvalidOperationException("A question cannot be answered more than once.");
            }

            var examQuestionIds = await _unitofwork.ExamQuestions.GetQuestionIdsByExamIdAsync(dto.ExamId);

            if (dto.Answers.Any(x => !examQuestionIds.Contains(x.QuestionId)))
            {
                throw new InvalidOperationException("One or more submitted questions do not belong to this exam.");
            }

            var submittedQuestionIds = dto.Answers
                .Select(x => x.QuestionId)
                .ToHashSet();

            if (!examQuestionIds.SetEquals(submittedQuestionIds))
            {
                throw new InvalidOperationException("You must answer all exam questions before submitting.");
            }

            int studentScore = 0;

            foreach (var answerDto in dto.Answers)
            {
                var option = await _unitofwork.QuestionOptions.GetByIdAndQuestionIdAsync(answerDto.OptionId, answerDto.QuestionId);
                if (option == null)
                {
                    throw new KeyNotFoundException($"There is no option with Id: {answerDto.OptionId} for question Id: {answerDto.QuestionId}");
                }

                var studentAnswer = new StudentExamAnswer
                {
                    ExamId = dto.ExamId,
                    QuestionId = answerDto.QuestionId,
                    OptionId = answerDto.OptionId,
                    StudentId = studentId,
                    IsCorrect = option.IsCorrect
                };

                if (studentAnswer.IsCorrect)
                {
                    studentScore++;
                }

                await _unitofwork.StudentExamAnswers.AddAsync(studentAnswer);
            }

            var result = new StudentExamResult
            {
                ExamId = dto.ExamId,
                StudentId = studentId,
                StudentScore = studentScore,
                ExamScore = examQuestionsCount
            };

            await _unitofwork.StudentExamResults.AddAsync(result);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
