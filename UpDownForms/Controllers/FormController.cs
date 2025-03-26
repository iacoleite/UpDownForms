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

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetForm(int id)
        {

            //if (id < 0)
            //{
            //    return StatusCode(353, "pocoto");
            //}
            var response = await _formService.GetForm(id);

            return Ok(response);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FormDTO>> PostForm([FromBody] CreateFormDTO createFormDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _formService.PostForm(createFormDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<FormDTO>> PutForm(int id, [FromBody] UpdateFormDTO updateFormDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _formService.PutForm(id, updateFormDTO);

            return Ok(response);

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteForm(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _formService.DeleteForm(id);

            return Ok(response);

        }
    }
}
