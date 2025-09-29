using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto
{
    public class HabitEditDto
    {
        [Required, MaxLength(100)]
        public String Name { get; set; } = String.Empty;
        [MaxLength(500)]
        public String? Description { get; set; }
        public Periodicity Periodicity { get; set; } = Periodicity.Daily;
        [Range(1, 90)]
        public int TargetPeriod { get; set; } = 1;
        public bool IsArchived { get; set; } = false;
        public HabitScheduleDto Schedule { get; set; }
    }
}
