using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.Entities;

namespace YemekhaneApp.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<MealRecord> MealRecords { get; set; }
        public DbSet<UserDebt> UserDebts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<MealRecord>().HasKey(m => m.Id);
            modelBuilder.Entity<UserDebt>().HasKey(ud => ud.Id);


            // Relationships
            modelBuilder.Entity<MealRecord>()
                .HasOne(m => m.Employee)
                .WithMany(e => e.MealRecords)
                .HasForeignKey(m => m.EmployeeId);


            modelBuilder.Entity<UserDebt>()
                .HasOne<Employee>()
                .WithMany(e => e.UserDebts)
                .HasForeignKey(ud => ud.EmployeeId);

            // Unique constraint: aynı gün aynı çalışana birden fazla kayıt olmasın
            //modelBuilder.Entity<MealRecord>()
            //    .HasIndex(m => new { m.EmployeeId, m.Date })
            //    .IsUnique();
        }
    }

}
