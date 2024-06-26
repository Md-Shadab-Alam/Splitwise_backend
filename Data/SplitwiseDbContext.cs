﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Splitwise.Entities;

namespace Splitwise.Data
{
    public class SplitwiseDbContext : IdentityDbContext<ApplicationUser>
    {
        public SplitwiseDbContext(DbContextOptions options) : base(options) { }
    
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupDetail> GroupDetail { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.LoginProvider,
                    e.ProviderKey
                });
            });
            //modelBuilder.Entity<Group>()
            //    .HasMany(g => g.Users)
            //    .UsingEntity(j => j.ToTable("GroupUser"));

            modelBuilder.Entity<Balance>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<ExpenseDetail>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");

            // Other configurations...
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Group>()
        //    //    .HasMany(g => g.Users)
        //    //    .UsingEntity(j => j.ToTable("GroupUser"));

        //    modelBuilder.Entity<Balance>()
        //        .Property(b => b.Amount)
        //        .HasColumnType("decimal(18,2)");


        //    modelBuilder.Entity<Expense>()
        //        .Property(b => b.Amount)
        //        .HasColumnType("decimal(18,2)");

        //    modelBuilder.Entity<Expense>()
        //        .HasOne(e => e.Users)
        //        .WithMany()
        //        .HasForeignKey(e => e.UsersId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //}


    }
}
