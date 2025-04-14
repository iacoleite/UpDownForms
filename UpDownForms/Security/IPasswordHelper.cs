using UpDownForms.Models;

namespace UpDownForms.Security
{
    public interface IPasswordHelper
    {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string password, string passwordHash);
    }

}
