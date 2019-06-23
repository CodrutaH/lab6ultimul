﻿// <auto-generated />
using System;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab2.Migrations
{
    [DbContext(typeof(ExpensesDbContext))]
    [Migration("20190602164559_MakeUsersAndRoleManyToMany")]
    partial class MakeUsersAndRoleManyToMany
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lab2.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ExpenseId");

                    b.Property<bool>("Important");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Lab2.Models.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Currency");

                    b.Property<System.DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Location");

                    b.Property<int?>("OwnerId");

                    b.Property<int>("Sum");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Lab2.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lab2.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Lab2.Models.UserUserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<System.DateTime?>("EndTime");

                    b.Property<System.DateTime>("StartTime");

                    b.Property<int>("UserId");

                    b.Property<int>("UserRoleId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("UserUserRoles");
                });

            modelBuilder.Entity("Lab2.Models.Comment", b =>
                {
                    b.HasOne("Lab2.Models.Expense", "Expense")
                        .WithMany("Comments")
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab2.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Lab2.Models.Expense", b =>
                {
                    b.HasOne("Lab2.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Lab2.Models.UserUserRole", b =>
                {
                    b.HasOne("Lab2.Models.User", "User")
                        .WithMany("UserUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab2.Models.UserRole", "UserRole")
                        .WithMany("UserUserRoles")
                        .HasForeignKey("UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

        }
    }
}
