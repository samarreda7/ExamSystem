using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class studentDto
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String PhoneNumber { get; set; }
        public Guid GroupId { get; set; }

    }
}
