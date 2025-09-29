using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicalHabitTracker.Model
{
    public class HabitSchedule
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Habit"), Required]
        public int HabitId { get; set; }

        public String TimeZoneId { get; set; }

        public TimeOnly PreferredLocalTime { get; set; }

        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;

        public int ReminderOffsetsMinutes { get; set; }

        public bool IsActive { get; set; } = false;

        public Habit Habit { get; set; } = null!;

        public DateTime? NextDueUtc { get; set; }

        public DateTime? SnoozeUntilUtc { get; set; }

    }

    [Flags]
    public enum WeeklyDaysMask
    {
        None = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Weekend = Sunday | Saturday,
        AllDays = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
    }
}
