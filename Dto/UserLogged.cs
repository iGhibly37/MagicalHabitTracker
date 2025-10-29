using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Dto
{
    public class UserLogged
    {

        public string Username { get; set; }

        public ICollection<HabitEditDto> Habits { get; set; } = new List<HabitEditDto>();
        public string Role { get; set; } = RoleEnum.User;


    }
}
