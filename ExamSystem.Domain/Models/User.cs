using ExamSystem.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Domain.Models
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
        public Role Role { get; set; }
        public Guid RoleId { get; set; }

    }
}
