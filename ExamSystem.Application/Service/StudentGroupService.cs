using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;


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
        public async Task<IEnumerable<ShowStudentDto>> GetStudentsByGroupIdAsync(Guid groupId, Guid teacherId)
        {
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
            var students = await _unitofwork.StudentGroup.GetStudentsByGroupIdAsync(groupId);
            return students.Select(s => new ShowStudentDto
            {
                Id = s.Student.UserId,
                FirstName = s.Student.User.FirstName,
                LastName = s.Student.User.LastName,
                PhoneNumber = s.Student.User.PhoneNumber,
                Email = s.Student.User.Email,
                Username = s.Student.User.Username,
            });
        }

        public async Task ReassignStudentToAnotherGroupAsync(Guid groupId, Guid studentId, Guid NewGroupId, Guid teacherId)
        {
            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }
            var Newgroup = await _unitofwork.Groups.GetByIdAsync(NewGroupId);
            if (Newgroup == null)
            {
                throw new KeyNotFoundException($"there is no group with this id {NewGroupId}");
            }
            var OldGroup = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (OldGroup == null)
            {
                throw new KeyNotFoundException($"there is no group with this id {groupId}");
            }
            var studentgroup = await _unitofwork.StudentGroup.GetStudentGroupAssign(studentId, groupId);
            if (studentgroup == null)
            {
                throw new InvalidOperationException("there is no assign with these Ids");
            }

            bool Isassignedbefore = await _unitofwork.StudentGroup.IsStudentAssignedToThisGroupAsync(studentId, NewGroupId);
            if (Isassignedbefore)
            {
                throw new InvalidOperationException("this student is assigned to this group before");
            }
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (Newgroup.TeacherUserId != teacherId || OldGroup.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("This teacher is not assigned to the target group.");
            }
            var NewStudentGroup = new StudentGroup
            {
                StudentId = studentId,
                GroupId = NewGroupId,
            };
            await _unitofwork.StudentGroup.DeleteAsync(studentgroup);
            await _unitofwork.StudentGroup.AddAsync(NewStudentGroup);
            await _unitofwork.SaveChangesAsync();
        }
        public async Task DeleteStudentAssignToGroupAsync(Guid studentId ,Guid groupId ,Guid teacherId)
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
            bool IsStudentAssigned = await _unitofwork.StudentGroup.IsStudentAssignedToThisGroupAsync(studentId, groupId);
            if (!IsStudentAssigned)
            {
                throw new InvalidOperationException($"There is no student with Id {studentId} assigned to group with Id {groupId}");
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
            var studentgroup = await _unitofwork.StudentGroup.GetStudentGroupAssign(studentId, groupId);
            if (studentgroup == null)
            {
                throw new InvalidOperationException("there is no assign with these Ids");
            }
            await _unitofwork.StudentGroup.DeleteAsync(studentgroup);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
