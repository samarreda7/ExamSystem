using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class AssignStudentToGroupDto
    {
        public Guid StudentId { get; set; }
        public Guid GroupId { get; set; }

    }
}
