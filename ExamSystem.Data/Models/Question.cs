using ExamSystem.Data.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid TeacherUserId { get; set; }
        public Teacher Teacher { get; set; }
        public List<ExamQuestion> ExamQuestions { get; set; }
        public List<QuestionOption> Options { get; set; }


    }
}
