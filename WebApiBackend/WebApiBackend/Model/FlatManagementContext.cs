using Microsoft.EntityFrameworkCore;
using System;

namespace WebApiBackend.Model
{
    public class FlatManagementContext : DbContext
    {
        public DbSet<TestModelItem> TestItems { get; set; }

        public DbSet<Flat> Flat { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<Schedule> Schedule { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserPayment> UserPayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=testdb.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity => {
                entity.Property(e => e.ScheduleType)
                    .HasConversion(
                        e => e.ToString(),
                        e => (ScheduleType)Enum.Parse(typeof(ScheduleType), e));
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.HasOne(e => e.Flat)
                    .WithMany(e => e.Users)
                    .HasForeignKey(e => e.FlatId);
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

            modelBuilder.Entity<UserPayment>(e => {
                e.HasKey(e => new { e.PaymentId, e.UserId });

                e.HasOne(e => e.Payment)
                    .WithMany(e => e.UserPayments)
                    .HasForeignKey(e => e.PaymentId);

                e.HasOne(e => e.User)
                    .WithMany(e => e.UserPayments)
                    .HasForeignKey(e => e.UserId);
            });
        }
    }
}
