using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.User;
using UpDownForms.Models;

namespace UpDownForms.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //private readonly ILogger<UserController> _logger;
    private readonly UpDownFormsContext _context;
    //public UserController(ILogger<UserController> logger)
    //{
    //    _logger = logger;
    //}

    public UserController(UpDownFormsContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDetailsDTO>>> GetUsers()
    {
        // Linq query to get all users that are not deleted
        return await _context.Users.Where(u => !u.IsDeleted).Cast<User>().Select(u => u.ToUserDetailsDTO()).ToListAsync();
            
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser(int id)
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

        var user = new User(createdUserDTO);
        //todo validation!
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDetailsDTO());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updatedUserDTO)
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
    public async Task<ActionResult<UserDetailsDTO>> DeleteUser(int id)
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
