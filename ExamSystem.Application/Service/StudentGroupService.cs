using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.Service
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IUnitOfWork _unitofwork;
        public StudentGroupService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task AssignStudentToGroupAsync(Guid studentId, Guid groupId, Guid teacherId)
        {
            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }
            var group = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"there is no group with this id {groupId}");
            }
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }
            if (group.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("This teacher is not assigned to the target group.");
            }
            bool Isduplicated = await _unitofwork.StudentGroup.IsStudentAssignedToThisGroupAsync(studentId, groupId);
            if (Isduplicated)
            {
                throw new InvalidOperationException("this student is assigned to this group before");
            }
            var studentGroup = new StudentGroup
            {
                GroupId = groupId,
                StudentId = studentId,
            };
            
            await _unitofwork.StudentGroup.AddAsync(studentGroup);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
