using Microsoft.EntityFrameworkCore;

namespace PersonalTaskManager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions options) : base (options) 
        {

        }
        
        public DbSet<TaskItem> TaskItems { get; set; }
    }

}
