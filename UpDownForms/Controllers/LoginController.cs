using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Security;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<string> Login([FromBody] LoginUserDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Invalid login data");
            }
            try
            {
                var token = await _loginService.Login(loginDTO);
                return token;
            }
            catch (ArgumentNullException ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                throw new EntityNotFoundException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedException(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
