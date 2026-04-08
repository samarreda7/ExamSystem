using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;


namespace ExamSystem.Application.Service
{
    public class ExamService :IExamService
    {
        private readonly IUnitOfWork _unitofwork;
        public ExamService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task AddExamAsync(Guid teacherId,CreateExamDto dto)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var exam = new Exam
            {
                Name = dto.Name,
                TeacherUserId = teacher.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _unitofwork.Exams.AddAsync(exam);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
