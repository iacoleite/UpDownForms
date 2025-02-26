using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        return await _context.Users.ToListAsync();

        //return new List<User>
        //{
        //    new User { Id = 1, Name = "Alice" },
        //    new User { Id = 2, Name = "Bob" },
        //    new User { Id = 3, Name = "Charlie" },
        //};
    }
    //public IEnumerable<WeatherForecast> Get()
    //{
    //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //        TemperatureC = Random.Shared.Next(-20, 55),
    //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //    })
    //    .ToArray();
    //}
}
