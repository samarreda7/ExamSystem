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
    public class QuestionOptionsEntityConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable("question_options");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();


            builder.Property(u => u.Text)
             .IsRequired()
             .HasMaxLength(200);

            builder.Property(q=>q.IsCorrect)
                .IsRequired();

            builder.Property(e => e.QuestionId).IsRequired();

            builder.HasOne(s => s.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(s => s.QuestionId);

        }
    }
}
