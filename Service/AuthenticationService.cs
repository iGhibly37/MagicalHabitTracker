using MagicalHabitTracker.Data;
using MagicalHabitTracker.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MagicalHabitTracker.Dto.UserDtos;
using System.Security.Cryptography;
using MagicalHabitTracker.Dto;


namespace MagicalHabitTracker.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext appDbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IConfiguration configuration;

        public AuthenticationService(
            IConfiguration configuration,
            AppDbContext appDbContext,
            IPasswordHasher<User> passwordHasher)
        {
            this.configuration = configuration;
            this.appDbContext = appDbContext;
            this.passwordHasher = passwordHasher;
        }
        public async Task<TokenResponseDto> LoginAsync(LoginUserDto userLoginDto, CancellationToken cancellationToken = default)
        {

            if (String.IsNullOrWhiteSpace(userLoginDto.Email.ToLower()) || String.IsNullOrWhiteSpace(userLoginDto.Username.ToLower()))
            {
                throw new ArgumentException("Email or Username cannot be empty.");
            }

            if (String.IsNullOrWhiteSpace(userLoginDto.Password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            User user = new User();

            if(userLoginDto.Email != null)
                user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userLoginDto.Email.ToLower(), cancellationToken);
            else if(userLoginDto.Username != null)
                user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == userLoginDto.Username.ToLower(), cancellationToken);


            if (user == null)
                throw new InvalidOperationException("Invalid email or password. Try again");

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid email or password. Try again");
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                string newHash = passwordHasher.HashPassword(user, userLoginDto.Password);
                user.PasswordHash = newHash;
                appDbContext.Users.Update(user);
            }

            await appDbContext.SaveChangesAsync();

            var ResponseDto = new TokenResponseDto
            {
                AccessToken = GenerateJwtToken(user),
                RefreshToken = await CreateAndSaveRefreshToken(user)
            };

            return ResponseDto;

        }

        //Il cancellation token serve per annullare l'operazione asincrona se necessario
        public async Task<User> RegistrationUserAsync(RegisterUserDto userRegistrationDto, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(userRegistrationDto.Email) || String.IsNullOrWhiteSpace(userRegistrationDto.Password) ||
                String.IsNullOrWhiteSpace(userRegistrationDto.Username) || String.IsNullOrWhiteSpace(userRegistrationDto.PhoneNumber))
            {
                throw new ArgumentException("Email, Password, Username and PhoneNumber cannot be null or empty.");
            }


            bool existsEmail = await appDbContext.Users.AnyAsync(u => u.Email == userRegistrationDto.Email, cancellationToken);
            bool userNameExists = await appDbContext.Users.AnyAsync(u => u.Username == userRegistrationDto.Username, cancellationToken);
            bool userNameExistsPhone = await appDbContext.Users.AnyAsync(u => u.PhoneNumber == userRegistrationDto.PhoneNumber, cancellationToken);

            if (existsEmail)
            {
                throw new InvalidOperationException("The email already exist. Try again");
            }
            if (userNameExists)
            {
                throw new InvalidOperationException("The username already exist. Try again");
            }

            if (userNameExistsPhone)
            {
                throw new InvalidOperationException("The phone number already exist. Try again");
            }

            var user = new User
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Email = userRegistrationDto.Email,
                Address = userRegistrationDto.Address,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                Username = userRegistrationDto.Username
            };

            string hash = passwordHasher.HashPassword(user, userRegistrationDto.Password);
            user.PasswordHash = hash;

            appDbContext.Users.Add(user);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow )
            {
                return null;
            }
            return user;
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> CreateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
            await appDbContext.SaveChangesAsync();
            return refreshToken;
        } 


        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );


            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var user = ValidateRefreshTokenAsync(refreshTokenRequestDto.Id, refreshTokenRequestDto.RefreshToken);
            if (user is null)
            {
                return null;
            }

            return CreateResponseToken(user);
        }

        private async Task<TokenResponseDto> CreateResponseToken(Task<User?> user)
        {
            var response = new TokenResponseDto
            {
                AccessToken = GenerateJwtToken(user.Result),
                RefreshToken = CreateAndSaveRefreshToken(user.Result).Result
            };

            return response;
        }
    }
}


  

