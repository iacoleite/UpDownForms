using System.Security.Claims;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly VerifyOwnershipHandler _verifyOwnershipHandler;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor, VerifyOwnershipHandler verifyOwnershipHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _verifyOwnershipHandler = verifyOwnershipHandler;
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

        public bool IsAuthorized(IVerifyOwnership resource)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext.User;
            var isAuthorized = _verifyOwnershipHandler.HandleRequirementAsync(user, OwnershipRequirement.IsOwner, resource);
            return true;
        }
    }
}