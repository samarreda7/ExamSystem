using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace ExamSystem.Application.Service
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitofwork;
        public StudentService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AddStudentAsync(CreateStudentDto user)
        {
            var passwordHasher = new PasswordHasher<User>();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            bool isEmailExist = await _unitofwork.Users.IsEmailExist(user.Email);
            if (isEmailExist)
            {
                throw new InvalidOperationException("Email is already exist");
            }
            bool isUsernameExist = await _unitofwork.Users.IsUsernameExist(user.Username);
            if (isUsernameExist)
            {
                throw new InvalidOperationException("Username is already exist");
            }
            var newuser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = UserType.Student,
            };
            newuser.PasswordHash = passwordHasher.HashPassword(newuser, user.Password);
            var student = new Student
            {
                UserId = newuser.Id,
            };
            await _unitofwork.Users.AddAsync(newuser);
            await _unitofwork.Students.AddAsync(student);
            await _unitofwork.SaveChangesAsync();

        }

        public async Task<IEnumerable<ShowStudentDto>> GetStudentsWithAllDetailsAsync()
        {
            var students = await _unitofwork.Students.GetAllWithDetailsAsync();
            return students.Select(s => new ShowStudentDto
            {
                Id = s.UserId,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                PhoneNumber = s.User.PhoneNumber,
                Email = s.User.Email,
                Username = s.User.Username,
               
            });
        }

        public async Task<ShowStudentDto> GetStudentByIdAsync(Guid id)
        {
            var student = await _unitofwork.Students.GetStudentDetailsById(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"There is no student with this {id}");
            }
            return new ShowStudentDto
            {
                Id = student.UserId,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                PhoneNumber = student.User.PhoneNumber,
                Email = student.User.Email,
                Username = student.User.Username,
                
            };
        }

        public async Task UpdateStudentAsync(Guid id, UpdateStudentDto dto)
        {
            var student = await _unitofwork.Students.GetStudentDetailsById(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"There is no student with this {id}");
            }
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Update data cannot be null.");
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

            student.User.FirstName = dto.FirstName;
            student.User.LastName = dto.LastName;
            student.User.PhoneNumber = dto.PhoneNumber;
            student.User.Email = dto.Email;
            student.User.Username = dto.Username;
            student.User.UpdatedAt = DateTime.UtcNow;

            await _unitofwork.Students.UpdateAsync(student);
            await _unitofwork.SaveChangesAsync();
        }
        public async Task DeleteStudentAsync(Guid id)
        {
            var student = await _unitofwork.Students.GetStudentDetailsById(id);

            if (student == null)
            {
                throw new KeyNotFoundException($"There is no student with this {id}");
            }
            await _unitofwork.Students.DeleteAsync(student);
            await _unitofwork.Users.DeleteAsync(student.User);
            await _unitofwork.SaveChangesAsync();
        }


    }

}
