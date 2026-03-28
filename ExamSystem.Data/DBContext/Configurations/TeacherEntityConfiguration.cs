using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {

            builder.ToTable("teachers");
            builder.HasKey(t=>t.UserId);
            builder.Property(u => u.UserId).ValueGeneratedNever();


            builder.HasOne(u => u.User)
                .WithOne(t => t.Teacher)
                .HasForeignKey<Teacher>(u=>u.UserId);


            builder.HasOne(t=>t.Subject)
                .WithMany(t => t.Teachers)
                .HasForeignKey(u=>u.SubjectId);


        }
    }
}
