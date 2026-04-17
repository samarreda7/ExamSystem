using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class StudentExamAnswerEntityConfiguration : IEntityTypeConfiguration<StudentExamAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentExamAnswer> builder)
        {
            builder.ToTable("student_exam_answers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ExamId)
                .IsRequired();

            builder.Property(x => x.QuestionId)
                .IsRequired();

            builder.Property(x => x.OptionId)
                .IsRequired();

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.IsCorrect)
                .IsRequired();

            builder.HasOne(x => x.Exam)
                .WithMany()
                .HasForeignKey(x => x.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Option)
                .WithMany()
                .HasForeignKey(x => x.OptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.ExamId, x.QuestionId, x.StudentId })
                .IsUnique();
        }
    }
}
