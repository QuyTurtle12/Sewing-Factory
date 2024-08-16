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

        //Mapping Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Task Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.tasks)
                .WithOne(t => t.user)
                .HasForeignKey(t => t.creatorID)
                .IsRequired();

            // User - Role Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.role)
                .WithMany(r => r.users)
                .HasForeignKey(u => u.roleID)
                .IsRequired();

            // User - Orders Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.orders)
                .WithOne(o => o.user)
                .HasForeignKey(o => o.userID)
                .IsRequired();

            // User - Group Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.group)
                .WithMany(g => g.users)
                .HasForeignKey(u => u.groupID)
                .IsRequired();

            // Group - Task Relationship
            modelBuilder.Entity<Group>()
                .HasMany(g => g.tasks)
                .WithOne(t => t.group)
                .HasForeignKey(t => t.groupID)
                .IsRequired();

            // Task - Order Relationship
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.order)
                .WithMany(o => o.tasks)
                .HasForeignKey(t => t.orderID)
                .IsRequired();

            // Order - Product Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.product)
                .WithMany(p => p.orders)
                .HasForeignKey(o => o.productID)
                .IsRequired();


            // Category - Product Relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.products)
                .WithOne(p => p.category)
                .HasForeignKey(p => p.categoryID)
                .IsRequired();

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { ID = Guid.NewGuid(), name = "Cashier" },
                new Role { ID = Guid.NewGuid(), name = "Order Manager" },
                new Role { ID = Guid.NewGuid(), name = "Product Manager" },
                new Role { ID = Guid.NewGuid(), name = "Staff Manager" },
                new Role { ID = Guid.NewGuid(), name = "Sewing Staff" }
            );
        }
    }
}
