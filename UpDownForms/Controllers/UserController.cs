using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.ApiResponse;
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
    public async Task<ActionResult> GetUsers()
    {
        var response = await _userService.GetUsers();
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Data);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser(string id)
    {
        var response = await _userService.GetUser(id);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<ActionResult> PostUser([FromBody] CreateUserDTO createdUserDTO)
    {  
        var response = await _userService.PostUser(createdUserDTO);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Message + "\n" + response.Data.Email);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserDetailsDTO>>> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        var response = await _userService.UpdateUser(id, updatedUserDTO);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Message + "\n" + response.Data.Email);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<UserDetailsDTO>>> DeleteUser(string id)
    {
        var response = await _userService.DeleteUser(id);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Message + "\n" + response.Data.Email);
    }

}
