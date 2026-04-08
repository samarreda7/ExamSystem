using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExamSystem.Data.DBContext.Configurations
{
    public class ExamEntityConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("exams");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(g => g.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(e => e.TeacherUserId)
                    .IsRequired();
            builder.HasOne(t=>t.Teacher)
                .WithMany(e=>e.Exams)
                .HasForeignKey(e=>e.TeacherUserId)
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
