using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Models
{
    public class Subject
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public List<Teacher> Teachers { get; set; }    
        public List<Group> Groups { get; set; }        
        public List<Question> Questions { get; set; }
    }
}
