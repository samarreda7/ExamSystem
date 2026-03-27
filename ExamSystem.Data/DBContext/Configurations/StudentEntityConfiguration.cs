using ExamSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");
            builder.HasKey(t => t.UserId);
            builder.Property(u => u.UserId).ValueGeneratedNever();

            builder.HasOne(u => u.User)
                .WithOne(s => s.Student)
                .HasForeignKey<Student>(u => u.UserId);

            builder.Property(g => g.GroupId)
                   .IsRequired();

            builder.HasOne(g=>g.Group)
                .WithMany(t=>t.Students)
                .HasForeignKey(g=>g.GroupId);

        }
    }
}
