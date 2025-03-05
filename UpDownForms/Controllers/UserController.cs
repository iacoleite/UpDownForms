using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services;

namespace UpDownForms.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //private readonly ILogger<UserController> _logger;
    private readonly UpDownFormsContext _context;
    private readonly IPasswordHelper _passwordHelper;
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;
    //public UserController(ILogger<UserController> logger)
    //{
    //    _logger = logger;
    //}

    public UserController(UserService userService, IPasswordHelper passwordHelper, UpDownFormsContext context, UserManager<User> userManager)
    {
        _context = context;
        _passwordHelper = passwordHelper;
        _userManager = userManager;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDetailsDTO>>> GetUsers()
    {
        // Linq query to get all users that are not deleted
        var users = await _userService.GetUsers();
        return Ok(users);
            
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser(string id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            return user;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostUser([FromBody] CreateUserDTO createdUserDTO)
    {  

        //if (createdUserDTO == null)
        //{
        //    return BadRequest("Missing user data");
        //}

        ////var user = new User(createdUserDTO);
        ////todo validation!
        //var user = createdUserDTO.ToUserDto();
        //var result = await _userManager.CreateAsync(user, createdUserDTO.Password);
        ////user.PasswordHash = _passwordHelper.HashPassword(user, createdUserDTO.Password);
        ////_context.Users.Add(user);
        //if (!result.Succeeded) 
        //{
        //    return BadRequest(result.Errors);
        //}
        
        //await _context.SaveChangesAsync();
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var result = await _userService.PostUser(createdUserDTO);
        if (!result.IdentityResult.Succeeded)
        {
            return BadRequest(result.IdentityResult.Errors);
        }
        
        return CreatedAtAction(nameof(GetUser), new { id = result.CreatedUser.Id }, result.CreatedUser);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        if (updatedUserDTO == null)
        {
            return BadRequest("Missing user data");
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        
        user.UpdateUser(updatedUserDTO);

        //user.IsDeleted = updatedUserDTO.IsDeleted;
        await _context.SaveChangesAsync();
        return Ok(user.ToUserDetailsDTO());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> DeleteUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.DeleteUser();
        await _context.SaveChangesAsync();
        //return Ok($"User deleted.\n" + user.ToUserDetailsDTO());
        return Ok(user.ToUserDetailsDTO());
    }

}
