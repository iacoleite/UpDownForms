using Microsoft.AspNetCore.Http;
using UpDownForms.Models;

namespace UpDownForms.Services.Interfaces
{
    public interface ILoggedUserService
    {
        string GetLoggedInUserId();
        Task<bool> IsAuthorized(IVerifyOwnership resource);

    }
}