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
    }
}
