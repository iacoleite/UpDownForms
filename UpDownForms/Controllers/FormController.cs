using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
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

        //[HttpGet]
        //public async Task<ActionResult<Pageable<FormDTO>>> GetForms()
        //{
        //    var response = await _formService.GetForms(pageParameters);

        //}

        [HttpGet]

        //[PaginatedHttpGetAttribute("GetForms")]
        public async Task<ActionResult<Pageable<FormDTO>>> GetForms([FromQuery] PageParameters pageParameters)
        {
            var response = await _formService.GetForms(pageParameters);
            this.AddPaginationMetadata(response, pageParameters);
                        
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetForm(int id)
        {
            var response = await _formService.GetForm(id);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}/responses")]
        //[Route("{id}/responses")]
        //[PaginatedHttpGetAttribute("GetResponsesByFormId")]
        public async Task<ActionResult<Pageable<ResponseDTO>>> GetResponsesByFormId(int id, [FromQuery] PageParameters pageParameters)
         {
            var response = await _formService.GetResponsesByFormId(id, pageParameters);
            this.AddPaginationMetadata(response, pageParameters);

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
            var response = await _formService.DeleteForm(id);

            return Ok(response);
        }

        
    }
}
