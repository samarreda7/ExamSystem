using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;

namespace ExamSystem.Application.DTO
{
    public class ShowQuestionDto
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public QuestionType Type { get; set; }
        public required string TeacherFirstName { get; set; }
        public required string TeacherLastName { get; set; }

    }
}
