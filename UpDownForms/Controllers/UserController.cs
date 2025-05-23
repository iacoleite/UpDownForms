using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
using UpDownForms.Security;
using UpDownForms.Services;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<Pageable<UserDetailsDTO>>> GetUsers([FromQuery] PageParameters pageParameters)
    {
        var response = await _userService.GetUsers(pageParameters);
        this.AddPaginationMetadata(response, pageParameters);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> GetUser(string id)
    {
        var response = await _userService.GetUser(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> PostUser([FromBody] CreateUserDTO createdUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var response = await _userService.PostUser(createdUserDTO);
        return Ok(response);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var response = await _userService.UpdateUser(id, updatedUserDTO);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> DeleteUser(string id)
    {

        var response = await _userService.DeleteUser(id);

        return Ok(response);
    }

}
