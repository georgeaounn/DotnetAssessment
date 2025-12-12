using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<AuditLog> AuditLogs => Set<Domain.Entities.AuditLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                    .HasIndex(p => p.Email)
                    .IsUnique();
            
            modelBuilder.Entity<Product>()
                    .HasIndex(p => p.Name)
                    .IsUnique();

            // Thank you Mohammad for this suggestion :)
            modelBuilder.Entity<Item>()
                    .Property(i => i.RowVersion)
                    .IsRowVersion();
        }
    }

}