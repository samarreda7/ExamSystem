namespace ExamSystem.Application.DTO
{
    public class UpdateQuestionOptionDto
    {
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
