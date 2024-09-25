using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Models.Entities.Task> Tasks { get; set; }
        
        public DbSet<TaskTag> TaskTags { get; set; }
    }
}
