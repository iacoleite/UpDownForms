using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Security;

namespace UpDownForms.Services
{
    public class LoginService
    {
        private readonly UpDownFormsContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly TokenService _tokenService;

        public LoginService(UpDownFormsContext context, IPasswordHelper passwordHelper, TokenService tokenService)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<string>> Login(LoginUserDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return new ApiResponse<string>(false, "Missing Login data", null);
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                return new ApiResponse<string>(false, "User not found", null);
            }
            if (!_passwordHelper.VerifyPassword(user, loginDTO.Password, user.PasswordHash))
            {
                return new ApiResponse<string>(false, "Wrong password", null); ;
            }
            return new ApiResponse<string>(true, "Token generated successfully", _tokenService.GenerateToken(user));
        }


    }
}
