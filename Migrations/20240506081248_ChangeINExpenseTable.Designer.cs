﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Splitwise.Data;

#nullable disable

namespace Splitwise.Migrations
{
    [DbContext(typeof(SplitwiseDbContext))]
    [Migration("20240506081248_ChangeINExpenseTable")]
    partial class ChangeINExpenseTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Splitwise.Entities.Balance", b =>
                {
                    b.Property<int>("BalanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BalanceId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("BalanceId");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("Splitwise.Entities.Expense", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseId"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("ExpenseId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Splitwise.Entities.ExpenseDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId")
                        .IsUnique();

                    b.ToTable("ExpenseDetail");
                });

            modelBuilder.Entity("Splitwise.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<int>("GroupDetailsId")
                        .HasColumnType("int");

                    b.HasKey("GroupId");

                    b.HasIndex("GroupDetailsId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Splitwise.Entities.GroupDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GroupDetail");
                });

            modelBuilder.Entity("Splitwise.Entities.Users", b =>
                {
                    b.Property<int>("UsersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsersId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsersId");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("GroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Splitwise.Entities.ExpenseDetail", b =>
                {
                    b.HasOne("Splitwise.Entities.Expense", null)
                        .WithOne("ExpenseDetails")
                        .HasForeignKey("Splitwise.Entities.ExpenseDetail", "ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Splitwise.Entities.Group", b =>
                {
                    b.HasOne("Splitwise.Entities.GroupDetail", "GroupDetails")
                        .WithMany()
                        .HasForeignKey("GroupDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupDetails");
                });

            modelBuilder.Entity("Splitwise.Entities.Users", b =>
                {
                    b.HasOne("Splitwise.Entities.Expense", null)
                        .WithMany("UsersInvolved")
                        .HasForeignKey("ExpenseId");

                    b.HasOne("Splitwise.Entities.Group", null)
                        .WithMany("Users")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("Splitwise.Entities.Expense", b =>
                {
                    b.Navigation("ExpenseDetails")
                        .IsRequired();

                    b.Navigation("UsersInvolved");
                });

            modelBuilder.Entity("Splitwise.Entities.Group", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}