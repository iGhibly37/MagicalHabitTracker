using System.ComponentModel.DataAnnotations;

namespace MagicalHabitTracker.Dto.UserDtos
{
    public class LoginUserDto
    {
        public string? Email { get; set; }

        public string? Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
