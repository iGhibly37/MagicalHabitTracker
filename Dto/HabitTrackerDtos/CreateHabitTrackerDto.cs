namespace MagicalHabitTracker.Dto.HabitTrackerDtos
{
    public class CreateHabitTrackerDto
    {
        public DateTime? CompletedAtUtc { get; set; }

        public int HabitId { get; set; }
    }
}
