using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Dto.UserDtos;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IAuthenticationService
    {
        public  Task<TokenResponseDto?> LoginAsync(LoginUserDto userLoginDto, CancellationToken ct = default);
        public  Task<User?> RegistrationUserAsync(RegisterUserDto userRegistrationDto, CancellationToken cancellationToken = default);

        public Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto);
    }
}
