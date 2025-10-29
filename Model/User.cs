using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicalHabitTracker.Model
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, MaxLength(100),   ]
        public string Email { get; set; }
        [Required, MaxLength(100),   ]
        public string PasswordHash { get; set; }

        [Required,   ]
        public string Role { get; set; } = RoleEnum.User;

        public Address Address { get; set; }

        [Required, MaxLength(15),   ]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(20),   ]
        public string Username { get; set; }

        public ICollection<Habit> Habits { get; set; } = new List<Habit>();

    }

    [Owned]
    public class Address {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }


    }

}
