using UpDownForms.DTO.UserDTOs;

namespace UpDownForms.Services.Interfaces
{
    public interface ILoginService
    {
        Task<string> Login(LoginUserDTO loginDTO);
    }
}