using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ExamSystem.Data.DBContext.Configurations
{
    public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("groups");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.SubjectId).IsRequired();
            builder.HasOne(t => t.Subject)
                .WithMany(e => e.Groups)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.TeacherUserId).IsRequired();
            builder.HasOne(t => t.Teacher)
                .WithMany(e => e.Groups)
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
