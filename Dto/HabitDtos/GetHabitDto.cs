using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Dto.HabitDtos
{
    public class GetHabitDto
    {

        public string Name { get; set; }

        public string? Description { get; set; } = string.Empty;

        public Periodicity Periodicity { get; set; } = Periodicity.Daily;
        public int TargetPeriod { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public GetHabitScheduleDto? HabitSchedule { get; set; }


    }
}
