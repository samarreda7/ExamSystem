using ExamSystem.Data.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String PasswordHash { get; set; }
        public String PhoneNumber { get; set; }
        public UserType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Teacher Teacher { get; set; }
        public Student Student { get; set; }

    }
}
