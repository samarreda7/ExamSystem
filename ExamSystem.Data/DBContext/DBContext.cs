using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



namespace ExamSystem.Data.DBContext
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<ExamQuestion> exam_questions { get; set; }
        public DbSet<ExamGroup> exam_groups { get; set; }
        public DbSet<Exam> exams { get; set; }
        public DbSet<Group> groups { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<QuestionOption> question_options { get; set; }
        public DbSet<StudentGroup> student_group { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<StudentExamAnswer> student_exam_answers { get; set; }
        public DbSet<StudentExamResult> student_exam_results { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
    }
}
