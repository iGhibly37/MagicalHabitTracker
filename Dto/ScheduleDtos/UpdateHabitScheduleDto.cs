using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Dto.ScheduleDtos
{
    public class UpdateHabitScheduleDto
    {

        public int HabitId { get; set; }
        public string TimeZoneId { get; set; } = string.Empty;
        public TimeOnly PreferredLocalTime { get; set; }
        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;
        public int ReminderOffsetsMinutes { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? NextDueUtc { get; set; }
    }
}
