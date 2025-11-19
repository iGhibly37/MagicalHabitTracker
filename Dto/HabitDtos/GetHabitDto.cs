using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;
using Microsoft.AspNetCore.SignalR;

namespace MagicalHabitTracker.Dto.HabitDtos
{
    public class GetHabitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; } = string.Empty;

        public Periodicity Periodicity { get; set; } = Periodicity.Daily;
        public int TargetPeriod { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public GetHabitScheduleDto? HabitSchedule { get; set; }

        public int UserId { get; set; }


    }
}
