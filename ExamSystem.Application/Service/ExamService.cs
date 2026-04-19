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
            if (dto.Name == null) 
            {
                throw new InvalidDataException("Exam name is required ");
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
        public async Task<IEnumerable<ShowExamDto>> GetTeacherExamsAsync(Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            var exams = await _unitofwork.Exams.GetAllTeacherExamAsync(teacherId);
            return exams.Select(exam => new ShowExamDto
            {
                Id = exam.Id,
                Name = exam.Name,
                TeacherFirstName = exam.Teacher.User.FirstName,
                TeacherLastName = exam.Teacher.User.LastName,
                QuestionsCount = exam.ExamQuestions.Count,
                GroupsCount = exam.ExamGroups.Count,
            });
        }

        public async Task UpdateExamAsync(Guid teacherId, Guid examId, UpdateExamDto dto)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            if (exam.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("this teacher is not Uthorized to update this exam");
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Name == null)
            {
                throw new InvalidDataException("Exam name is required ");
            }

            exam.Name = dto.Name;
            exam.UpdatedAt = DateTime.UtcNow;

            await _unitofwork.Exams.UpdateAsync(exam);
            await _unitofwork.SaveChangesAsync();
        }

        public async Task DeleteExamAsync(Guid teacherId, Guid examId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if(exam == null)
            {
                throw new KeyNotFoundException($"There i no exams with this Id {examId}");
            }
            if(exam.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("this teacher is not Uthorized to delete this exam");
            }
            await _unitofwork.Exams.DeleteAsync(exam);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
