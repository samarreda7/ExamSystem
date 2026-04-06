using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class ReassignStudentToAnotherGroupDto
    {
        public Guid StudentId { get; set; }
        public Guid GroupId { get; set; }
        public Guid newGroupId { get; set; }


    }
}
