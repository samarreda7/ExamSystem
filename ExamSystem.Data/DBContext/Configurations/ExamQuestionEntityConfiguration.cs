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
    public class ExamQuestionEntityConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.ToTable("exam_questions");
            builder.HasKey(s => new { s.ExamId, s.QuestionId });

            builder.HasOne(s => s.Exam)
                   .WithMany(e => e.ExamQuestions)
                   .HasForeignKey(s => s.ExamId);

            builder.HasOne(s => s.Question)
                   .WithMany(q => q.ExamQuestions)
                   .HasForeignKey(s => s.QuestionId);
        }
    }
}
