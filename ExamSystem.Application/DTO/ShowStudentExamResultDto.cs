namespace ExamSystem.Application.DTO
{
    public class ShowStudentExamResultDto
    {
        public Guid ExamId { get; set; }
        public string ExamName { get; set; }
        public Guid StudentId { get; set; }
        public int StudentScore { get; set; }
        public int ExamScore { get; set; }
    }
}
