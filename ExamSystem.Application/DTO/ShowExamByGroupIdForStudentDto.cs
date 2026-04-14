namespace ExamSystem.Application.DTO
{
    public class ShowExamByGroupIdForStudentDto
    {
        public Guid ExamId { get; set; }
        public required string ExamName { get; set; }
        public required string SubjectName { get; set; }
    }
}
