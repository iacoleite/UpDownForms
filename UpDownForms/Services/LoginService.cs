using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Security;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUpDownFormsContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ITokenService _tokenService;

        public LoginService(IUpDownFormsContext context, IPasswordHelper passwordHelper, ITokenService tokenService)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _tokenService = tokenService;
        }

        public async Task<string> Login(LoginUserDTO loginDTO)
        {
            if (loginDTO == null)
            {
                throw new BadHttpRequestException("Invalid login data");

            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                throw new EntityNotFoundException("Invalid user Id");
            }
            if (!_passwordHelper.VerifyPassword(user, loginDTO.Password, user.PasswordHash))
            {
                throw new BadHttpRequestException("Wrong password");

            }
            return _tokenService.GenerateToken(user);
        }


    }
}
