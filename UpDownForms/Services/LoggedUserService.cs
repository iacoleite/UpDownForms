using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
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

        public async Task<bool> IsAuthorized(IVerifyOwnership resource)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext.User;
            var isAuthorized = await _authorizationService.AuthorizeAsync(user, resource, OwnershipRequirement.IsOwner);
            var teste = isAuthorized.Succeeded;
            return teste;
        }
    }
}