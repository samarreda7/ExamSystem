using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExamSystem.Data.DBContext.Configurations
{
    public class ExamGroupEntityConfiguration : IEntityTypeConfiguration<ExamGroup>
    {
        public void Configure(EntityTypeBuilder<ExamGroup> builder)
        {
            builder.ToTable("exam_groups");
            builder.HasKey(s => new { s.ExamId, s.GroupId });

            builder.HasOne(s => s.Exam)
                   .WithMany(e => e.ExamGroups)
                   .HasForeignKey(s => s.ExamId);

            builder.HasOne(s => s.Group)
                   .WithMany(q => q.ExamGroups)
                   .HasForeignKey(s => s.GroupId);
        }
    }
}
