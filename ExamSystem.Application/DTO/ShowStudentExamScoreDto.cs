namespace ExamSystem.Application.DTO
{
    public class ShowStudentExamScoreDto
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public Guid StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int StudentScore { get; set; }
        public int ExamScore { get; set; }
    }
}
