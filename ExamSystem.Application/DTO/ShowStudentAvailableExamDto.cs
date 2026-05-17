namespace ExamSystem.Application.DTO
{
    public class ShowStudentAvailableExamDto
    {
        public Guid ExamId { get; set; }
        public required string ExamName { get; set; }
        public required string TeacherName { get; set; }
        public required string SubjectName { get; set; }
        public int QuestionsCount { get; set; }
    }
}
