using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Models
{
    public class ExamGroup
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }

    }
}
