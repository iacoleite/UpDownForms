using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.Models;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly FormService _formService;

        public FormController(FormService formService)
        {
            _formService = formService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetForms()
        {
            var response = await _formService.GetForms();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetForm(int id)
        {
            var response = await _formService.GetForm(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FormDTO>> PostForm([FromBody] CreateFormDTO createFormDTO)
        {
            var response = await _formService.PostForm(createFormDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data.Title);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<FormDTO>> PutForm(int id, [FromBody] UpdateFormDTO updateFormDTO)
        {
            //var userId = _userService.GetLoggedInUserId();
            //var form = await _context.Forms.FindAsync(id);
            //if (userId != form.UserId)
            //{
            //    return Unauthorized("You are not the owner of this form");
            //}
            var response = await _formService.PutForm(id, updateFormDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data.Title);

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteForm(int id)
        {
            //var userId = _userService.GetLoggedInUserId();
            //var form = await _context.Forms.FindAsync(id);
            //if (userId != form.UserId)
            //{
            //    return Unauthorized("You are not the owner of this form");
            //}
            var response = await _formService.DeleteForm(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);

        }
    }
}
