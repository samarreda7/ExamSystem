namespace ExamSystem.Application.DTO
{
    public class ShowStudentExamScoreDto
    {
        public Guid StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int StudentScore { get; set; }
        public int ExamScore { get; set; }
    }
}
