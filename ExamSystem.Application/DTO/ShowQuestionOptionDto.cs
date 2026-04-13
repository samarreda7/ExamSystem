namespace ExamSystem.Application.DTO
{
    public class ShowQuestionOptionDto
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
    }
}
