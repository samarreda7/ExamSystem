using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class StudentExamResultEntityConfiguration : IEntityTypeConfiguration<StudentExamResult>
    {
        public void Configure(EntityTypeBuilder<StudentExamResult> builder)
        {
            builder.ToTable("student_exam_results");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ExamId)
                .IsRequired();

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.StudentScore)
                .IsRequired();

            builder.Property(x => x.ExamScore)
                .IsRequired();

            builder.HasOne(x => x.Exam)
                .WithMany()
                .HasForeignKey(x => x.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.ExamId, x.StudentId })
                .IsUnique();
        }
    }
}
