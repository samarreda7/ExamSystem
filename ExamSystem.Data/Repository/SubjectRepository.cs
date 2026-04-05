using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ExamSystem.Data.Repository
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository 
    {
        public SubjectRepository(AppDBContext context) : base(context) { }



        public async Task<bool> IsSubjectExistAsync(Guid Id)
        {
            return await _dbSet.AnyAsync(x => x.Id == Id);
        }
        public async Task<bool> IsSubjectNameExistAsync(string subjectName)
        {
            return await _dbSet.AnyAsync(x => x.Name == subjectName) ;
        }
     
    }
}
