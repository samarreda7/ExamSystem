using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.Models
{
    public class Exam
    {
        public Guid Id { get; set; }
        public Guid TeacherUserId { get; set; }
        public Teacher Teacher { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ExamQuestion> ExamQuestions { get; set; }
        public List<ExamGroup> ExamGroups { get; set; }


    }
}
