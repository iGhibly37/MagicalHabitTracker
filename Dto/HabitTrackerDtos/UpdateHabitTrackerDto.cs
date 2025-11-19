namespace MagicalHabitTracker.Dto.HabitTrackerDtos
{
    public class UpdateHabitTrackerDto
    {

        public DateTime? CompletedAtUtc { get; set; }

        public int HabitId { get; set; }
    }
}
