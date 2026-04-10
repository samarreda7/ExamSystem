using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTO
{
    public class CreateQuestionDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public Guid SubjectId { get; set; }

    }
}
