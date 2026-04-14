using ExamSystem.Domain.ValueTypes;

namespace ExamSystem.Application.DTO
{
    public class ShowQuestionByExamIdDto
    {
        public Guid QuestionId { get; set; }
        public required string Text { get; set; }
        public QuestionType Type { get; set; }
    }
}
