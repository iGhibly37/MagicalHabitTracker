using MagicalHabitTracker.Dto.UserDtos;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IAuthenticationService
    {
        public  Task<string> LoginAsync(LoginUserDto userLoginDto, CancellationToken ct = default);
        public  Task<User> RegistrationUserAsync(RegisterUserDto userRegistrationDto, CancellationToken cancellationToken = default);
    }
}
