using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Identity;


namespace ExamSystem.Application.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitofwork;
        public TeacherService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task AddTeacherAsync(CreateTeacherDto teacherDto)
        {
            var passwordHasher = new PasswordHasher<User>();
            if (teacherDto == null)
            {
                throw new ArgumentNullException(nameof(teacherDto));
            }
            bool isEmailExist = await _unitofwork.Users.IsEmailExist(teacherDto.Email);
            if (isEmailExist)
            {
                throw new InvalidOperationException("Email is already exist");
            }
            bool isUsernameExist = await _unitofwork.Users.IsUsernameExist(teacherDto.Username);
            if (isUsernameExist)
            {
                throw new InvalidOperationException("Username is already exist");
            }
            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(teacherDto.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"There is no subject with Id: {teacherDto.SubjectId}");
            }
            var newuser = new User
            {
                FirstName = teacherDto.FirstName,
                LastName = teacherDto.LastName,
                PhoneNumber = teacherDto.PhoneNumber,
                Username = teacherDto.Username,
                Email = teacherDto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = UserType.Teacher,
            };
            newuser.PasswordHash = passwordHasher.HashPassword(newuser, teacherDto.Password);
            var teacher = new Teacher
            {
                UserId = newuser.Id,
                SubjectId = teacherDto.SubjectId,
            };
            await _unitofwork.Users.AddAsync(newuser);
            await _unitofwork.Teachers.AddAsync(teacher);
            await _unitofwork.SaveChangesAsync();

        }
        public async Task<IEnumerable<ShowTeacherDto>> GetTeachersWithAllDetailsAsync()
        {
            var teachers = await _unitofwork.Teachers.GetAllWithDetailsAsync();
            return teachers.Select(t => new ShowTeacherDto
            {
                Id = t.UserId,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                PhoneNumber = t.User.PhoneNumber,
                Email = t.User.Email,
                Username = t.User.Username,
                SubjectName = t.Subject.Name,
                GroupsCount = t.Groups.Count,
                ExamsCount = t.Exams.Count,
            });
        }
        public async Task<ShowTeacherDto> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await _unitofwork.Teachers.GetTeacherDetailsById(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"There is no teacher with this {id}");
            }
            return new ShowTeacherDto
            {
                Id = teacher.UserId,
                FirstName = teacher.User.FirstName,
                LastName = teacher.User.LastName,
                PhoneNumber = teacher.User.PhoneNumber,
                Email = teacher.User.Email,
                Username = teacher.User.Username,
                SubjectName = teacher.Subject.Name,
                GroupsCount = teacher.Groups.Count,
                ExamsCount = teacher.Exams.Count,
            };
        }
        public async Task DeleteTeacherAsync(Guid id)
        {
            var teacher = await _unitofwork.Teachers.GetTeacherDetailsById(id);
            var user = await _unitofwork.Users.GetByIdAsync(id);

            if (teacher == null)
            {
                throw new KeyNotFoundException($"There is no teacher with this {id}");
            }

            if (user == null)
            {
                throw new KeyNotFoundException($"There is no user with this {id}");
            }
            if (teacher.Groups != null && teacher.Groups.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete this teacher because they are assigned to one or more groups."
                );
            }
            foreach (var exam in teacher.Exams)
            {
                await _unitofwork.Exams.DeleteAsync(exam);
            }
            foreach (var question in teacher.Questions)
            {
                await _unitofwork.Questions.DeleteAsync(question);
            }

            await _unitofwork.Teachers.DeleteAsync(teacher);
            await _unitofwork.Users.DeleteAsync(user);
            await _unitofwork.SaveChangesAsync();
        }
        public async Task UpdateTeacherAsync(Guid id, UpdateTeacherDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Update data cannot be null.");
            }
            var teacher = await _unitofwork.Teachers.GetTeacherDetailsById(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"There is no teacher with this {id}");
            }
            bool isEmailExist = await _unitofwork.Users.IsEmailExistForAnotherUser(dto.Email, id);
            if (isEmailExist)
            {
                throw new InvalidOperationException("Email is already exist");
            }
            bool isUsernameExist = await _unitofwork.Users.IsUsernameExistForAnotherUser(dto.Username, id);
            if (isUsernameExist)
            {
                throw new InvalidOperationException("Username is already exist");
            }
            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(dto.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"There is no subject with Id: {dto.SubjectId}");
            }
            teacher.User.FirstName = dto.FirstName;
            teacher.User.LastName = dto.LastName;
            teacher.User.Username = dto.Username;
            teacher.User.Email = dto.Email;
            teacher.User.PhoneNumber = dto.PhoneNumber;
            teacher.SubjectId = dto.SubjectId;
            teacher.User.UpdatedAt = DateTime.UtcNow;

            await _unitofwork.Teachers.UpdateAsync(teacher);
            await _unitofwork.SaveChangesAsync();

        }
        public async Task<IEnumerable<ShowTeacherDto>> GetTeachersBySubjectIdAsync(Guid subjectId)
        {
            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(subjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"There is no subject with Id: {subjectId}");
            }
            var teachers = await _unitofwork.Teachers.GetTeachersBySubjectIdAsync(subjectId);
            if (!teachers.Any())
            {
                throw new KeyNotFoundException($"There are no teachers for this subject Id: {subjectId}");
            }
            return teachers.Select(t => new ShowTeacherDto
            {
                Id = t.UserId,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                PhoneNumber = t.User.PhoneNumber,
                Email = t.User.Email,
                Username = t.User.Username,
                SubjectName = t.Subject.Name,
                GroupsCount = t.Groups.Count,
                ExamsCount = t.Exams.Count,
            });
        }
    }
}
