using System;

namespace ExamSystem.Domain.Models
{
    public class StudentExamResult
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public int StudentScore { get; set; }
        public int ExamScore { get; set; }
    }
}
