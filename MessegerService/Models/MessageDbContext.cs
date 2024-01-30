using Microsoft.EntityFrameworkCore;

namespace MessegerService.Models
{
    public class MessageDbContext : DbContext
    {
        private static string _connectionString;
        public MessageDbContext()
        {

        }

        public MessageDbContext(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public DbSet<MessageEntity> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SenderId);
                entity.Property(x => x.RecipientId);

                entity.Property(e => e.Text)
                    .HasMaxLength(1000);
            });
        }
    }
}
