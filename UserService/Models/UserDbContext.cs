using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    public class UserDbContext : DbContext
    {
        private static string _connectionString;
        public UserDbContext()
        {

        }

        public UserDbContext(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_connectionString)
                //.UseSqlServer(_connectionString)
                .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Login).IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasOne(x => x.Role)
                    .WithMany(u => u.Users);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name);

            });
        }
    }
}
