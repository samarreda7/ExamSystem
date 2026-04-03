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
    }
}
