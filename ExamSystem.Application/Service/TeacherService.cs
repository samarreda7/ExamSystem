using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
