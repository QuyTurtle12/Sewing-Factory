﻿using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.Models;

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
        public DbSet<Models.Models.Task> Tasks { get; set; }

        // Mapping Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Task Relationship
            modelBuilder.Entity<User>()
                .HasMany<Models.Models.Task>()
                .WithOne()
                .HasForeignKey(t => t.CreatorID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User - Role Relationship
            modelBuilder.Entity<User>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User - Orders Relationship
            modelBuilder.Entity<User>()
                .HasMany<Order>()
                .WithOne()
                .HasForeignKey(o => o.UserID)
                .IsRequired();

            // User - Group Relationship
            modelBuilder.Entity<User>()
                .HasOne<Group>()
                .WithMany()
                .HasForeignKey(u => u.GroupID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Group - Task Relationship
            modelBuilder.Entity<Group>()
                .HasMany<Models.Models.Task>()
                .WithOne()
                .HasForeignKey(t => t.GroupID)
                .IsRequired();

            // Task - Order Relationship
            modelBuilder.Entity<Models.Models.Task>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(t => t.OrderID)
                .IsRequired();

            // Order - Product Relationship
            modelBuilder.Entity<Order>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(o => o.ProductID)
                .IsRequired();

            // Category - Product Relationship
            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

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
