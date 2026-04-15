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
        public async Task<ShowGroupDto> AddGroupAsync(CreateGroupDto groupDto)
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
            bool isTeacherExist = await _unitofwork.Teachers.IsTeacherExistAsync(groupDto.TeacherId);
            if (!isTeacherExist)
            {
                throw new KeyNotFoundException($"No teacher found with Id: {groupDto.TeacherId}");
            }

            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectExistAsync(groupDto.SubjectId);
            if (!isSubjectExist)
            {
                throw new KeyNotFoundException($"No subject found with Id: {groupDto.SubjectId}");
            }
            var group = new Group
            {
                Name = groupDto.Name,
                TeacherUserId=groupDto.TeacherId,
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
    }
}
