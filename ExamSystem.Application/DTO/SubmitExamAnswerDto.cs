namespace ExamSystem.Application.DTO
{
    public class SubmitExamAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
    }
}
