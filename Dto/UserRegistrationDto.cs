using MagicalHabitTracker.Model;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicalHabitTracker.Dto
{
    public class UserRegistrationDto
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Address? Address { get; set; }
        [Required, MaxLength(15),   ]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(20)]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; } = RoleEnum.User;
    }
}
