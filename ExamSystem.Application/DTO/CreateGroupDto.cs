using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class CreateGroupDto
    {
        public string Name { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }

    }
}
