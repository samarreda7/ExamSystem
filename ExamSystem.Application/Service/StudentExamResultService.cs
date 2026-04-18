using ExamSystem.Application.IService;
using ExamSystem.Application.DTO;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Application.Service
{
    public class StudentExamResultService : IStudentExamResultService
    {
        private readonly IUnitOfWork _unitofwork;

        public StudentExamResultService(IUnitOfWork unitofwork)
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

            var examGroups = (await _unitofwork.ExamGroups.GetByExamAsync(dto.ExamId)).ToList();
            if (!examGroups.Any())
            {
                throw new InvalidOperationException("This exam is not assigned to any group.");
            }

            var studentGroupIds = (await _unitofwork.StudentGroup.GetGroupsByStudentIdAsync(studentId))
                .Select(x => x.GroupId)
                .ToHashSet();

            bool canAccessExam = examGroups.Any(x => studentGroupIds.Contains(x.GroupId));
            if (!canAccessExam)
            {
                throw new UnauthorizedAccessException("This student is not assigned to a group that has this exam.");
            }

            var existingResult = await _unitofwork.StudentExamResults.GetAllAsync();
            if (existingResult.Any(x => x.ExamId == dto.ExamId && x.StudentId == studentId))
            {
                throw new InvalidOperationException("This student has already submitted this exam.");
            }

            var examQuestions = (await _unitofwork.ExamQuestions.GetByExamAsync(dto.ExamId)).ToList();
            if (!examQuestions.Any())
            {
                throw new InvalidOperationException("This exam has no questions assigned.");
            }

            if (dto.Answers.Count != examQuestions.Count)
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

            var examQuestionIds = examQuestions
                .Select(x => x.QuestionId)
                .ToHashSet();

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
                var option = await _unitofwork.QuestionOptions.GetByIdAsync(answerDto.OptionId);
                if (option == null)
                {
                    throw new KeyNotFoundException($"There is no option with Id: {answerDto.OptionId}");
                }

                if (option.QuestionId != answerDto.QuestionId)
                {
                    throw new InvalidOperationException("The selected option does not belong to the submitted question.");
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
                ExamScore = examQuestions.Count
            };

            await _unitofwork.StudentExamResults.AddAsync(result);
            await _unitofwork.SaveChangesAsync();
        }

        public async Task<ShowStudentExamResultDto> GetStudentResultByStudentIdAndExamIdAsync(Guid studentId, Guid examId)
        {
            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var result = await _unitofwork.StudentExamResults.GetByStudentIdAndExamIdAsync(studentId, examId);

            if (result == null)
            {
                throw new KeyNotFoundException("There is no result for this student in this exam.");
            }

            return new ShowStudentExamResultDto
            {
                ExamId = result.ExamId,
                ExamName = exam.Name,
                StudentId = result.StudentId,
                StudentScore = result.StudentScore,
                ExamScore = result.ExamScore
            };
        }
    }
}
