

namespace ExamSystem.Application.DTO
{
    public class ShowExamDto
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public int QuestionsCount { get; set;}
        public int GroupsCount { get; set; }

    }
}
