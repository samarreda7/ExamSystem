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
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository 
    {
        public SubjectRepository(AppDBContext context) : base(context) { }



        public async Task<bool> IsSubjectExistAsync(Guid subjectId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == subjectId) != null;
        }
    }
}
