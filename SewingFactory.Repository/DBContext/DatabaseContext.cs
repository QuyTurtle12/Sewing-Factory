using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;

namespace SewingFactory.Repositories.DBContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        // Mapping Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Task Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.CreatorID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User - Role Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User - Orders Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserID)
                .IsRequired();

            // User - Group Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Group - Task Relationship
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Tasks)
                .WithOne(t => t.Group)
                .HasForeignKey(t => t.GroupID)
                .IsRequired();

            // Task - Order Relationship
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Tasks)
                .HasForeignKey(t => t.OrderID)
                .IsRequired();

            // Order - Product Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductID)
                .IsRequired();

            // Category - Product Relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID)
                .IsRequired();

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { ID = Guid.NewGuid(), Name = "Cashier" },
                new Role { ID = Guid.NewGuid(), Name = "Order Manager" },
                new Role { ID = Guid.NewGuid(), Name = "Product Manager" },
                new Role { ID = Guid.NewGuid(), Name = "Staff Manager" },
                new Role { ID = Guid.NewGuid(), Name = "Sewing Staff" }
            );
        }
    }
}
