namespace ExamSystem.Application.DTO
{
    public class SubmitExamDto
    {
        public Guid ExamId { get; set; }
        public List<SubmitExamAnswerDto> Answers { get; set; } = new();
    }
}
