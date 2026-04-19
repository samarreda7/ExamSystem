using ExamSystem.Application.IService;
using ExamSystem.Application.DTO;
using ExamSystem.Domain.IRepository;

namespace ExamSystem.Application.Service
{
    public class StudentExamResultService : IStudentExamResultService
    {
        private readonly IUnitOfWork _unitofwork;

        public StudentExamResultService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
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

        public async Task<IEnumerable<ShowStudentExamScoreDto>> GetStudentScoresByGroupIdAndExamIdAsync(Guid teacherId, Guid groupId, Guid examId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            var groupExists = await _unitofwork.Groups.IsGroupExistAsync(groupId);
            if (!groupExists)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            bool isGroupOwnedByTeacher = await _unitofwork.Groups.IsGroupOwnedByTeacherAsync(groupId, teacherId);
            if (!isGroupOwnedByTeacher)
            {
                throw new UnauthorizedAccessException("You can only view scores for your own groups.");
            }

            bool isExamAssignedToGroup = await _unitofwork.ExamGroups.IsExamAssignedToGroupAsync(examId, groupId);
            if (!isExamAssignedToGroup)
            {
                throw new InvalidOperationException("This exam is not assigned to this group.");
            }

            var studentGroups = (await _unitofwork.StudentGroup.GetStudentsByGroupIdAsync(groupId)).ToList();
            var studentIds = studentGroups.Select(x => x.StudentId).ToList();

            var results = (await _unitofwork.StudentExamResults.GetByExamIdAndStudentIdsAsync(examId, studentIds))
                .ToDictionary(x => x.StudentId, x => x);

            int examScore = results.Values.FirstOrDefault()?.ExamScore
                ?? await _unitofwork.ExamQuestions.CountByExamIdAsync(examId);

            return studentGroups.Select(studentGroup =>
            {
                results.TryGetValue(studentGroup.StudentId, out var result);

                return new ShowStudentExamScoreDto
                {
                    StudentId = studentGroup.StudentId,
                    StudentFirstName = studentGroup.Student.User.FirstName,
                    StudentLastName = studentGroup.Student.User.LastName,
                    StudentScore = result?.StudentScore ?? 0,
                    ExamScore = result?.ExamScore ?? examScore
                };
            });
        }
    }
}
