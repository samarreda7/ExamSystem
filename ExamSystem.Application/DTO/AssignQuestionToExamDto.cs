namespace ExamSystem.Application.DTO
{
    public class AssignQuestionToExamDto
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
