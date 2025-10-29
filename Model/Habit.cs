using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicalHabitTracker.Model
{

    public class Habit
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public String Name { get; set; }

        [MaxLength(500)]
        public String? Description { get; set; }

        public Periodicity Periodicity { get; set; } = Periodicity.Daily;

        [Range(0, 31)]
        public int DayofMonth { get; set; }

        [Range(1, 90)]
        public int TargetPeriod { get; set; } = 1;

        public bool IsArchived { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public HabitSchedule? HabitSchedule { get; set; }

        public ICollection<Tracker> Trackers { get; set; } = new List<Tracker>();

        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }

    }

    public enum Periodicity
    {
        Daily,
        Weekly,
        Monthly
    }
}
