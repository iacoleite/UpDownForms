using UpDownForms.Models;

namespace UpDownForms.Security
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}