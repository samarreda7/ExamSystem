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
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository 
    {
        public TeacherRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Teacher>> GetAllWithDetailsAsync()
        {
            return await _dbSet.Include(s => s.User)
                .Include(s => s.Subject)
                .Include(e=>e.Exams)
                .Include(g => g.Groups)
                .ToListAsync();
        }

    }
}
