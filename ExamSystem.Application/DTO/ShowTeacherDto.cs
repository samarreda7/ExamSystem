using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class ShowTeacherDto
    {
        public Guid Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String PhoneNumber { get; set; }
        public string SubjectName { get; set; }
        public int GroupsCount { get; set; }
        public int ExamsCount { get; set; }
    }
}
