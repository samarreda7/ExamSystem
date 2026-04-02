using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class CreateStudentDto
    {
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String Email { get; set; }
        public required String Username { get; set; }
        public required String Password { get; set; }
        public required String PhoneNumber { get; set; }

    }
}
