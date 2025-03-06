using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UpDownForms.Services
{
    public interface IUserService
    {
        string GetLoggedInUserId();
    }

    public class LoggedUserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
    }
}
