using MagicalHabitTracker.Dto.HabitDtos;
using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto.ScheduleDtos
{
    public class CreateHabitScheduleDto
    {
        public string TimeZoneId { get; set; } = string.Empty;

        public TimeOnly PreferredLocalTime { get; set; }

        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;

        public int ReminderOffsetsMinutes { get; set; }

        public bool IsActive { get; set; } = false;

        
        public DateTime? NextDueUtc { get; set; }

        public DateTime? SnoozeUntilUtc { get; set; }

        public int HabitId { get; set; }

    }
}
