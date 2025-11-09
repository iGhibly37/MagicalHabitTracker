using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto
{
    public class UserLoginDto
    {
        public string? Email { get; set; }

        public string? Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
