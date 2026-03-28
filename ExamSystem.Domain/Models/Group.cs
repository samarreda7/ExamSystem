using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
        public Guid TeacherUserId { get; set; }
        public Teacher Teacher { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Student> Students { get; set; }
        public List<ExamGroup> ExamGroups  { get; set; }
    }
}
