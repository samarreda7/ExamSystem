using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Identity;

namespace ExamSystem.Application.Service
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitofwork;
        public StudentService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AddStudentAsync(studentDto user)
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
            bool isGroupExist = await _unitofwork.Groups.IsGroupExistAsync(user.GroupId);
            if (!isGroupExist)
            {
                throw new KeyNotFoundException($"There is no group with Id: {user.GroupId}");
            }
            var newuser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Username =user.Username,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = UserType.Student,
            };
            newuser.PasswordHash = passwordHasher.HashPassword(newuser, user.Password);
            var student = new Student
            {
                UserId = newuser.Id,
                GroupId = user.GroupId,
            };
          await  _unitofwork.Users.AddAsync(newuser);
          await  _unitofwork.Students.AddAsync(student);
          await  _unitofwork.SaveChangesAsync();

        }
       
    }
}
