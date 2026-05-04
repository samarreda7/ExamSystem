using ExamSystem.Application.DTO;
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
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitofwork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<ShowGroupDto> AddGroupAsync(Guid teacherId, CreateGroupDto groupDto)
        {
            if (groupDto == null)
            {
                throw new ArgumentNullException(nameof(groupDto));
            }
            bool isGroupExist = await _unitofwork.Groups.IsGroupNameExistAsync(groupDto.Name);
            if (isGroupExist)
            {
                throw new InvalidDataException("A Group with this name already exists.");
            }
            bool isTeacherExist = await _unitofwork.Teachers.IsTeacherExistAsync(teacherId);
            if (!isTeacherExist)
            {
                throw new KeyNotFoundException($"No teacher found with Id: {teacherId}");
            }

            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(groupDto.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"No subject found with Id: {groupDto.SubjectId}");
            }
            var group = new Group
            {
                Name = groupDto.Name,
                TeacherUserId=teacherId,
                SubjectId= groupDto.SubjectId,
            };

            await _unitofwork.Groups.AddAsync(group);
            await _unitofwork.SaveChangesAsync();

            var subject = await _unitofwork.Subjects.GetByIdAsync(group.SubjectId);

            return new ShowGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                TeacherId = group.TeacherUserId,
                SubjectName = subject?.Name ?? string.Empty
            };
        }

        public async Task<IEnumerable<ShowGroupDto>> GetAllGroupsAsync()
        {
            var groups = await _unitofwork.Groups.GetAllGroupsAsync();
            return groups.Select(s => new ShowGroupDto
            {
                Id = s.Id,
                Name = s.Name,
                TeacherId=s.Teacher.UserId,
                SubjectName=s.Subject.Name,       

            });

        }

        public async Task<ShowGroupDto> GetGroupByIdAsync(Guid groupId)
        {
            var group = await _unitofwork.Groups.GetGroupByIdWithDetailsAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"There is no group with Id: {groupId}");
            }

            return new ShowGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                TeacherId = group.TeacherUserId,
                SubjectName = group.Subject.Name,
            };
        }

        public async Task<int> GetGroupsCountByTeacherIdAsync(Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            return await _unitofwork.Groups.GetGroupsCountByTeacherIdAsync(teacherId);
        }

        public async Task<IEnumerable<ShowGroupDto>> GetTeacherGroupsAsync(Guid teacherId)
        {
            var teacher = await _unitofwork.Teachers.GetByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException($"there is no teacher with this Id {teacherId}");
            }

            var groups = await _unitofwork.Groups.GetGroupsByTeacherIdAsync(teacherId);
            return groups.Select(s => new ShowGroupDto
            {
                Id = s.Id,
                Name = s.Name,
                TeacherId = s.TeacherUserId,
                SubjectName = s.Subject.Name,
            });
        }
    }
}
