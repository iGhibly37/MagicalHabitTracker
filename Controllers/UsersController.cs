using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicalHabitTracker.Data;
using MagicalHabitTracker.Model;
using MagicalHabitTracker.Service;
using Microsoft.AspNetCore.Authorization;
using MagicalHabitTracker.Dto.UserDtos;
using MagicalHabitTracker.Dto;

namespace MagicalHabitTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(AppDbContext context, IAuthenticationService authenticationService)

        {
            _context = context;
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(RegisterUserDto userDto)
        {
            var registered = await _authenticationService.RegistrationUserAsync(userDto);
            if (registered == null)
                return BadRequest("Username/Email already registered.");


            return CreatedAtAction(nameof(RegisterUser), new { email = userDto.Email }, userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> LoginUser(LoginUserDto loginDto)
        {
            var token = await _authenticationService.LoginAsync(loginDto);

            if (token == null)
                return BadRequest("Invalid username or password.");
            
            return Ok(token);
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto tokenRequest)
        {
            var tokenResponse = await _authenticationService.RefreshTokenAsync(tokenRequest);
            if (tokenResponse is null || tokenResponse.RefreshToken is null | tokenResponse.RefreshToken is null)
                return BadRequest("Invalid token.");

            return Ok(tokenResponse);
        }

        [Authorize]
        [HttpGet("authenticate")]
        public IActionResult AuthenticateService()
        {
            return Ok("Authentication service is running.");
        }
    }
}
