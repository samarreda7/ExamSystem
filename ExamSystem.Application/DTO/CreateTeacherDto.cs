using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class CreateTeacherDto
    {
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String Email { get; set; }
        public required String Username { get; set; }
        public required String Password { get; set; }
        public required String PhoneNumber { get; set; }
        public required Guid SubjectId { get; set; }
    }
}
