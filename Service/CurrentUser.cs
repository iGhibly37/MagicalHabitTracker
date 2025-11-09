using MagicalHabitTracker.Model;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MagicalHabitTracker.Service
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId => int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) 
                ? userId 
                : null;

        int ICurrentUser.UserId => throw new NotImplementedException();
    }
}
