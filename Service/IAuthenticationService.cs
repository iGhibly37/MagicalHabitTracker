using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IAuthenticationService
    {
        public  Task<string> LoginAsync(UserLoginDto userLoginDto, CancellationToken ct = default);
        public  Task<User> RegistrationUserAsync(UserRegistrationDto userRegistrationDto, CancellationToken cancellationToken = default);
    }
}
