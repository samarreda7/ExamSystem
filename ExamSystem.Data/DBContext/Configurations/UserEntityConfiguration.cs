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
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                    .ValueGeneratedNever();


            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.Username)
                   .IsRequired();

            builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();


            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(256)
                   .IsUnicode();

            builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

            builder.Property(u => u.Type)
                   .IsRequired()
                   .HasConversion<int>();

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
