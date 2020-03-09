using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Model
{
    public class FlatManagementContext : DbContext
    {
        public DbSet<TestModelItem> TestItems { get; set; }

        public DbSet<Flat> Flat { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<Schedule> Schdule { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=testdb.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity => {
            entity.HasKey(e => new { e.UserName, e.StartDate, e.EndDate, e.ScheduleType });

            entity.Property(e => e.ScheduleType)
                .HasConversion(
                    e => e.ToString(),
                    e => (ScheduleType)Enum.Parse(typeof(ScheduleType), e));
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();
            });

            modelBuilder.Entity<Payment>(e => {
                e.Property(e => e.PaymentType)
                    .HasConversion(
                        e => e.ToString(),
                        e => (PaymentType)Enum.Parse(typeof(PaymentType), e));

                e.Property(e => e.Frequency)
                    .HasConversion(
                        e => e.ToString(),
                        e => (Frequency)Enum.Parse(typeof(Frequency), e));
            });
        }
    }
}
