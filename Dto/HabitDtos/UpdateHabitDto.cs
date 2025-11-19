using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto.HabitDtos
{
    public class UpdateHabitDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; } = null;
        public Periodicity Periodicity { get; set; } = Periodicity.Daily;
        [Range(1, 90)]
        public int TargetPeriod { get; set; } = 1;
        public bool IsArchived { get; set; } = false;
        public CreateHabitScheduleDto? Schedule { get; set; }

    }
}
