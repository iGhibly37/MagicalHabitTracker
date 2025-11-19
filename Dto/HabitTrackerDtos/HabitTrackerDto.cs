using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto.HabitTrackerDtos
{
    public class HabitTrackerDto
    {

        public DateTime? CompletedAtUtc { get; set; }

        public int HabitId { get; set; }

        public int Id { get; set; }
    }
}
