using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Security;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly UpDownFormsContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly TokenService _tokenService;

        public LoginController(UpDownFormsContext context, IPasswordHelper passwordHelper, TokenService tokenService)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Missing login data");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (!_passwordHelper.VerifyPassword(user, loginDTO.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid password");
            }
            return Ok(_tokenService.GenerateToken(user));
        }

    }
}
