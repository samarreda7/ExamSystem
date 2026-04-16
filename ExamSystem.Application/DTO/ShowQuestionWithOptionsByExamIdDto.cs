using ExamSystem.Domain.ValueTypes;

namespace ExamSystem.Application.DTO
{
    public class ShowQuestionWithOptionsByExamIdDto
    {
        public Guid QuestionId { get; set; }
        public required string Text { get; set; }
        public QuestionType Type { get; set; }
        public IEnumerable<ShowQuestionOptionDto> Options { get; set; } = Enumerable.Empty<ShowQuestionOptionDto>();
    }
}
