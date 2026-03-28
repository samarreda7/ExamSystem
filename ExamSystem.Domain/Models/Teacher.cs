using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Models
{
    public class Teacher
    {
        public Guid  UserId { get; set; } //PK and FK
        public User User { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
        public List<Exam> Exams { get; set; }
        public List<Question> Questions { get; set; }
        public List<Group> Groups { get; set; }

    }
}
