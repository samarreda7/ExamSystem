using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;

namespace ExamSystem.Application.Service
{
    public class ExamGroupService : IExamGroupService
    {
        private readonly IUnitOfWork _unitofwork;

        public ExamGroupService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AssignExamToGroupAsync(Guid teacherId, AssignExamToGroupDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var exam = await _unitofwork.Exams.GetByIdAsync(dto.ExamId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {dto.ExamId}");
            }

            var group = await _unitofwork.Groups.GetByIdAsync(dto.GroupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {dto.GroupId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId || group.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only assign your own exams to your own groups.");
            }

            var examGroup = await _unitofwork.ExamGroups.GetByIdAsync(dto.ExamId, dto.GroupId);
            if (examGroup != null)
            {
                throw new InvalidOperationException("This exam is already assigned to this group.");
            }

            var newExamGroup = new ExamGroup
            {
                ExamId = dto.ExamId,
                GroupId = dto.GroupId
            };

            await _unitofwork.ExamGroups.AddAsync(newExamGroup);
            await _unitofwork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShowExamByGroupIdForStudentDto>> GetExamsByGroupIdAsync(Guid studentId, Guid groupId)
        {
            var student = await _unitofwork.Students.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"there is no student with this id {studentId}");
            }

            var group = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            bool isStudentAssignedToGroup = await _unitofwork.StudentGroup.IsStudentAssignedToThisGroupAsync(studentId, groupId);
            if (!isStudentAssignedToGroup)
            {
                throw new UnauthorizedAccessException("This student is not assigned to the target group.");
            }

            var examGroups = await _unitofwork.ExamGroups.GetByGroupAsync(groupId);

            return examGroups.Select(eg => new ShowExamByGroupIdForStudentDto
            {
                ExamId = eg.ExamId,
                ExamName = eg.Exam.Name,
                SubjectName = eg.Group.Subject.Name
            });
        }

        public async Task<int> GetExamCountByGroupIdAsync(Guid teacherId, Guid groupId)
        {
            var group = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (group.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only check your own groups.");
            }

            return await _unitofwork.ExamGroups.GetExamCountByGroupIdAsync(groupId);
        }

        public async Task<bool> IsExamAssignedToGroupAsync(Guid teacherId, Guid examId, Guid groupId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var group = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId || group.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only check your own exams and your own groups.");
            }

            return await _unitofwork.ExamGroups.IsExamAssignedToGroupAsync(examId, groupId);
        }

        public async Task RemoveExamFromGroupAsync(Guid teacherId, Guid examId, Guid groupId)
        {
            var exam = await _unitofwork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                throw new KeyNotFoundException($"There is no exam with Id: {examId}");
            }

            var group = await _unitofwork.Groups.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            if (exam.TeacherUserId != teacherId || group.TeacherUserId != teacherId)
            {
                throw new UnauthorizedAccessException("You can only remove your own exams from your own groups.");
            }

            var examGroup = await _unitofwork.ExamGroups.GetByIdAsync(examId, groupId);
            if (examGroup == null)
            {
                throw new InvalidOperationException("This exam is not assigned to this group.");
            }

            await _unitofwork.ExamGroups.DeleteAsync(examGroup);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
