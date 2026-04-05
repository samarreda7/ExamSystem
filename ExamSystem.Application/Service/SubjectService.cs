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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitofwork;
        public SubjectService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task AddSubjectAsync(CreateSubjectDto subjectDto)
        {
            if (subjectDto == null)
            {
                throw new ArgumentNullException(nameof(subjectDto));
            }
            bool isSubjectExist = await _unitofwork.Subjects.IsSubjectNameExistAsync(subjectDto.Name);
            if (isSubjectExist)
            {
                throw new InvalidDataException("A subject with this name already exists.");
            }
            var subject = new Subject
            {
                Name = subjectDto.Name,
            };

            await _unitofwork.Subjects.AddAsync(subject);
            await _unitofwork.SaveChangesAsync();
        }
    }
}
