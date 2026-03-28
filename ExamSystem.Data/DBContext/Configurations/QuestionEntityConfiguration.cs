using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExamSystem.Data.DBContext.Configurations
{
    public class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("questions");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.Text)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Type)
              .IsRequired()
              .HasConversion<int>();


            builder.Property(e => e.SubjectId).IsRequired();
            builder.HasOne(t => t.Subject)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.SubjectId);

            builder.Property(e => e.TeacherUserId).IsRequired();
            builder.HasOne(t => t.Teacher)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.TeacherUserId)
                    .OnDelete(DeleteBehavior.NoAction);



            builder.Property(u => u.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
