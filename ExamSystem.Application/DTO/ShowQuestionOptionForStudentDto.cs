namespace ExamSystem.Application.DTO
{
    public class ShowQuestionOptionForStudentDto
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public Guid QuestionId { get; set; }
    }
}
