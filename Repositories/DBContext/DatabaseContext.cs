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
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        //Mapping Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Composition Key Configuration
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.orderID, od.productID });

            //Relationship Mapping
            modelBuilder.Entity<User>()
                        .HasMany(t => t.tasks)
                        .WithOne(u => u.user)
                        .IsRequired()
                        .HasForeignKey(t => t.creatorID);

            modelBuilder.Entity<User>()
                        .HasOne(r => r.role)
                        .WithMany(u => u.users)
                        .IsRequired()
                        .HasForeignKey(u => u.roleID);

            modelBuilder.Entity<User>()
                        .HasMany(o => o.orders)
                        .WithOne(u => u.user)
                        .IsRequired()
                        .HasForeignKey(o => o.userID);

            modelBuilder.Entity<User>()
                        .HasOne(g => g.group)
                        .WithMany(u => u.users)
                        .IsRequired()
                        .HasForeignKey(u => u.groupID);

            modelBuilder.Entity<Group>()
                        .HasMany(t => t.tasks)
                        .WithOne(g => g.group)
                        .IsRequired()
                        .HasForeignKey(t => t.groupID);

            modelBuilder.Entity<Models.Task>()
                        .HasOne(o => o.order)
                        .WithMany(t => t.tasks)
                        .IsRequired()
                        .HasForeignKey(t => t.orderID);

            modelBuilder.Entity<Order>()
                        .HasMany(od => od.detail)
                        .WithOne(o => o.order)
                        .IsRequired()
                        .HasForeignKey(od => od.orderID);

            modelBuilder.Entity<Product>()
                        .HasMany(od => od.detail)
                        .WithOne(p => p.product)
                        .IsRequired()
                        .HasForeignKey(od => od.productID);

            modelBuilder.Entity<Category>()
                        .HasMany(p => p.products)
                        .WithOne(c => c.category)
                        .IsRequired()
                        .HasForeignKey(p => p.categoryID);

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
