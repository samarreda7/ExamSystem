using System;

namespace ExamSystem.Domain.Models
{
    public class StudentExamAnswer
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid OptionId { get; set; }
        public QuestionOption Option { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public bool IsCorrect { get; set; }
    }
}
