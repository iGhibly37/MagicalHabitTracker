using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto
{
    public class HabitTrackerDto
    {
        public bool IsCompleted { get; set; }

        [Required]
        public DateOnly Date {  get; set; }

        public DateTime? CompletedAtUtc { get; set; }

        public Habit Habit { get; set; }
    }
}
