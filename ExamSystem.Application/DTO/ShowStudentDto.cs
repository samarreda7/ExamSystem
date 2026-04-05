using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class ShowStudentDto
    {
        public required Guid Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public required String Email { get; set; }
        public required String Username { get; set; }
        public String PhoneNumber { get; set; }
    }
}
