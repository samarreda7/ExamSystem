using ExamSystem.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExamSystem.Data.DBContext  
{
    public class DBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=SAMAR;Initial Catalog=ExamSystemDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;"
            );
            return new AppDBContext(optionsBuilder.Options);
        }
    }
}