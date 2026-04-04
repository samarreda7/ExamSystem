using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamSystem.Domain.ValueTypes;
using ExamSystem.Domain.Constants;

namespace ExamSystem.Data.DBContext.Configurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r=>r.Name).IsUnique();
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasData(
                   new Role { Id = Guid.Parse(RolesId.AdminId), Name = RoleName.Admin.ToString() },
                   new Role { Id = Guid.Parse(RolesId.TeacherId), Name = RoleName.Teacher.ToString() },
                   new Role { Id = Guid.Parse(RolesId.StudentId), Name = RoleName.Student.ToString() });
        }
    }
}
