using Microsoft.EntityFrameworkCore;

namespace VideoServiceApiApp.Models
{
    public class VideoServiceContext : DbContext
    {
        public VideoServiceContext(DbContextOptions<VideoServiceContext> options)
            : base(options)
        {
        }

        public DbSet<VideoItem> VideoItems { get; set; }
    }
}
