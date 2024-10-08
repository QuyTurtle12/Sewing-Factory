﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SewingFactory.Repositories.DBContext;

#nullable disable

namespace SewingFactory.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240816113631_ModifiedProperties")]
    partial class ModifiedProperties
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SewingFactory.Models.Category", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SewingFactory.Models.Group", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SewingFactory.Models.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("FinishedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProductID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("TotalAmount")
                        .HasColumnType("float");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SewingFactory.Models.Product", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SewingFactory.Models.Role", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            ID = new Guid("9d621dc0-ff6f-47b7-87e2-758383f4b13f"),
                            Name = "Cashier"
                        },
                        new
                        {
                            ID = new Guid("b4811f59-6537-41a9-890d-d41f8a7475a8"),
                            Name = "Order Manager"
                        },
                        new
                        {
                            ID = new Guid("3af9bfa0-a6a0-4fb9-b3f4-77a87cf670c0"),
                            Name = "Product Manager"
                        },
                        new
                        {
                            ID = new Guid("fd831fa3-53fb-49d5-8521-58b8e2964a35"),
                            Name = "Staff Manager"
                        },
                        new
                        {
                            ID = new Guid("f624ee68-31e2-465b-a719-120120c4fecf"),
                            Name = "Sewing Staff"
                        });
                });

            modelBuilder.Entity("SewingFactory.Models.Task", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GroupID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("Status")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CreatorID");

                    b.HasIndex("GroupID");

                    b.HasIndex("OrderID");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("SewingFactory.Models.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("Salary")
                        .HasColumnType("float");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("GroupID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SewingFactory.Models.Order", b =>
                {
                    b.HasOne("SewingFactory.Models.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SewingFactory.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SewingFactory.Models.Product", b =>
                {
                    b.HasOne("SewingFactory.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("SewingFactory.Models.Task", b =>
                {
                    b.HasOne("SewingFactory.Models.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SewingFactory.Models.Group", "Group")
                        .WithMany("Tasks")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SewingFactory.Models.Order", "Order")
                        .WithMany("Tasks")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SewingFactory.Models.User", b =>
                {
                    b.HasOne("SewingFactory.Models.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SewingFactory.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SewingFactory.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("SewingFactory.Models.Group", b =>
                {
                    b.Navigation("Tasks");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("SewingFactory.Models.Order", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("SewingFactory.Models.Product", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("SewingFactory.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("SewingFactory.Models.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
