using Microsoft.EntityFrameworkCore;

namespace MyApi
{
    public class AppDbContext: DbContext
    {
        public DbSet<FileRecord> Files { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
