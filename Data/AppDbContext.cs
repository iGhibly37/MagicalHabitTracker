using MagicalHabitTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicalHabitTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitSchedule> Schedules { get; set; }
        public DbSet<HabitTracker> HabitTrackers { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Habit>()
                .HasIndex(h => h.Name);

            modelBuilder.Entity<Habit>()
                .HasOne(h => h.HabitSchedule)
                .WithOne(hs => hs.Habit)
                .HasForeignKey<HabitSchedule>(hs => hs.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Habit>()
                .HasMany(h => h.Trackers)
                .WithOne(t => t.Habit)
                .HasForeignKey(t => t.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HabitSchedule>()
                .HasIndex(hs => hs.HabitId)
                .IsUnique();

            modelBuilder.Entity<HabitTracker>()
                .HasIndex(t => new { t.HabitId, t.CompletedAtUtc })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(h => h.Habits)
                .WithOne(u => u.User)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                        .OwnsOne(u => u.Address)
                        .HasIndex("ZipCode")
                        .HasDatabaseName("IX_User_Address_ZipCode");

            modelBuilder.Entity<Habit>()
                .HasIndex(h => new {h.Name, h.UserId});



        }

    }
}
