using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.Models
{
    public class Student
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
       
    }
}
