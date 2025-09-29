using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto
{
    public class HabitScheduleDto
    {
        public String TimeZoneId { get; set; } = String.Empty;

        public TimeOnly PreferredLocalTime { get; set; }

        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;

        [Range(0,31)]
        public int DayOfMonth { get; set; } 

        public int ReminderOffsetsMinutes { get; set; }

        public bool IsActive { get; set; } = false;

        public Habit Habit { get; set; } = null!;

        public DateTime? NextDueUtc { get; set; }

        public DateTime? SnoozeUntilUtc { get; set; }

    }
}
