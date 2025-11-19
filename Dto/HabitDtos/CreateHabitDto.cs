using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicalHabitTracker.Dto.HabitDtos
{
    public class CreateHabitDto
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public Periodicity Periodicity { get; set; } = Periodicity.Daily;
        [Range(1, 90)]
        public int TargetPeriod { get; set; } = 1;
        //public CreateHabitScheduleDto Schedule { get; set; }

    }
}
