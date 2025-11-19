using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto.ScheduleDtos
{
    public class GetHabitScheduleDto
    {
        public string TimeZoneId { get; set; } = string.Empty;

        public TimeOnly PreferredLocalTime { get; set; }

        public WeeklyDaysMask WeeklyDaysMask { get; set; } = WeeklyDaysMask.AllDays;

        public bool IsActive { get; set; } = false;

        public DateTime? NextDueUtc { get; set; }
    }
}
