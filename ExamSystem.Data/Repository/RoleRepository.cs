using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDBContext context) : base(context) { }
        public async Task<Role?> GetRoleName(string name)
        {
            return await _context.roles.FirstOrDefaultAsync(r => r.Name == name);
        }

    }
}
