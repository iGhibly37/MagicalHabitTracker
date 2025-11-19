using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicalHabitTracker.Model
{
    public class HabitTracker
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Habit"), Required]
        public int HabitId { get; set; }

        public Habit Habit { get; set; }

        public DateTime CompletedAtUtc { get; set; } = DateTime.UtcNow;        

    }
}
