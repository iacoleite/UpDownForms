using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;

namespace UpDownForms.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //private readonly ILogger<UserController> _logger;
    private readonly UpDownFormsContext _context;
    private readonly IPasswordHelper _passwordHelper;
    //public UserController(ILogger<UserController> logger)
    //{
    //    _logger = logger;
    //}

    public UserController(UpDownFormsContext context, IPasswordHelper passwordHelper)
    {
        _context = context;
        _passwordHelper = passwordHelper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDetailsDTO>>> GetUsers()
    {
        // Linq query to get all users that are not deleted
        return await _context.Users.Where(u => !u.IsDeleted).Cast<User>().Select(u => u.ToUserDetailsDTO()).ToListAsync();
            
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user.ToUserDetailsDTO();
    }

    [HttpPost]
    public async Task<ActionResult<UserDetailsDTO>> PostUser([FromBody] CreateUserDTO createdUserDTO)
    {  

        if (createdUserDTO == null)
        {
            return BadRequest("Missing user data");
        }

        //var user = new User(createdUserDTO);
        //todo validation!
        var user = createdUserDTO.ToUserDto();
        user.PasswordHash = _passwordHelper.HashPassword(user, createdUserDTO.Password);
        _context.Users.Add(user);
        
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDetailsDTO());
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
