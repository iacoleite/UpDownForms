using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO;
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
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserDTO>> PostUser([FromBody] CreateUserDTO createdUserDTO)
    {

        if (createdUserDTO == null)
        {
            return BadRequest("Missing user data");
        }

        var user = new User(createdUserDTO);
        //todo validation!
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}
