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
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderID, od.productID });

        }
    }
}
