using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicalHabitTracker.Model
{
    public class HabitSchedule
    {

        [Key]
        public int Id { get; set; }

        public String TimeZoneId { get; set; }

        public TimeOnly PreferredLocalTime { get; set; }

        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;

        public MonthlyMask MonthlyDaysMask { get; set; } = MonthlyMask.None;

        public int ReminderOffsetsMinutes { get; set; }

        public bool IsActive { get; set; } = false;

        [ForeignKey("Habit"), Required]
        public int HabitId { get; set; }

        public Habit Habit { get; set; } = null!;

        public DateTime? NextDueUtc { get; set; }

        public DateTime? SnoozeUntilUtc { get; set; }

    }

    [Flags]
    public enum MonthlyMask
    {
        None = 0,
        January = 1,
        February = 2,
        March = 4,
        April = 8,
        May = 16,
        June = 32,
        July = 64,
        August = 128,
        September = 256,
        October = 512,
        November = 1024,
        December = 2048,
        All = January | February | March | April | May | June |
                    July | August | September | October | November | December

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
