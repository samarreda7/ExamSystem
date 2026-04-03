using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class StudentGroupEntityConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
         public void Configure(EntityTypeBuilder<StudentGroup> builder)
         {
            builder.ToTable("students_group");
            builder.HasKey(s => new { s.StudentId, s.GroupId });

            builder.HasOne(s => s.Student)
                   .WithMany(e => e.StudentGroups)
                   .HasForeignKey(s => s.StudentId);

            builder.HasOne(s => s.Group)
                   .WithMany(q => q.StudentGroups)
                   .HasForeignKey(s => s.GroupId);
        }
    }
}
