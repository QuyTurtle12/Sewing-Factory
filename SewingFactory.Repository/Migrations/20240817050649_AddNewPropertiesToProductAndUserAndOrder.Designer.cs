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
    [Migration("20240817050649_AddNewPropertiesToProductAndUserAndOrder")]
    partial class AddNewPropertiesToProductAndUserAndOrder
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

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerPhone")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

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
                            ID = new Guid("7bc584c7-7c67-4601-a418-f63f85966e07"),
                            Name = "Cashier"
                        },
                        new
                        {
                            ID = new Guid("226106cb-c589-4ecf-8a66-ebeaefc9eb2b"),
                            Name = "Order Manager"
                        },
                        new
                        {
                            ID = new Guid("f626ae14-39e2-4b6d-abd9-03656243b2ac"),
                            Name = "Product Manager"
                        },
                        new
                        {
                            ID = new Guid("a9a731cb-fb75-4f36-a615-681caa0c2b46"),
                            Name = "Staff Manager"
                        },
                        new
                        {
                            ID = new Guid("42c2e00f-54d4-4d07-8688-d00449b1b430"),
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

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

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