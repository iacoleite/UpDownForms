using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Models;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : Controller
    {

        private readonly ResponseService _responseService;

        public ResponseController(ResponseService responseService)
        {

            _responseService = responseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDTO>>> GetResponses()
        {
            var response = await _responseService.GetResponses();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseFormNoResponseDTO>> GetResponse(int id)
        {
            var response = await _responseService.GetResponseById(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> PostResponse([FromBody] CreateResponseDTO createResponseDTO)
        {
            var response = await _responseService.PostResponse(createResponseDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResponse(int id)
        {

            var response = await _responseService.DeleteResponse(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("{id}/answers/")]
        public async Task<ActionResult<AnswerDTO>> PostAnswer(int id, [FromBody] CreateAnswerDTO createAnswerDTO)
        {

            var response = await _responseService.PostAnswer(id, createAnswerDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}

