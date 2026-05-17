using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class ShowGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }


    }
}
