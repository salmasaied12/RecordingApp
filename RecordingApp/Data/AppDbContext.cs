using Microsoft.EntityFrameworkCore;

namespace RecordingApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AudioModel> AudioFiles { get; set; }
    }
}
