using Microsoft.EntityFrameworkCore;
using ServiceSchedule.Infrastructure.Entities;

namespace ServiceSchedule.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Topic).IsRequired();
                e.Property(x => x.Data).IsRequired();
                e.Property(x => x.Key).IsRequired();
            });

        }
    }
}
