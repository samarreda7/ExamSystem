namespace ExamSystem.Application.DTO
{
    public class CreateQuestionOptionDto
    {
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
    }
}
