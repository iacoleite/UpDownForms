using Microsoft.AspNetCore.Http;

namespace UpDownForms.Services.Interfaces
{
    public interface ILoggedUserService
    {
        string GetLoggedInUserId();
    }
}