using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpDownForms.DTO.OptionDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;
using UpDownForms.Services;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {

        private readonly QuestionService _questionService;

        public QuestionController(QuestionService questionService)
        {

            _questionService = questionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDetailsDTO>>> GetQuestions()
        {
            var response = await _questionService.GetQuestions();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestionById(int id)
        {
            var response = await _questionService.GetQuestionById(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionDetailsDTO>> PostQuestion([FromBody] CreateQuestionDTO createQuestionDTO)
        {
            var response = await _questionService.PostQuestion(createQuestionDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDTO>> PutQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            var response = await _questionService.PutQuestion(id, updateQuestionDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDTO>> DeleteQuestion(int id)
        {
            var response = await _questionService.DeleteQuestion(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        // Handling options
        [HttpGet("{questionId}/options")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetOptions(int questionId)
        {
            var response = await _questionService.GetOptionsByQuestion(questionId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost("{questionId}/options")]
        public async Task<IActionResult> AddOption(int questionId, [FromBody] CreateOptionDTO createOptionDTO)
        {
            var response = await _questionService.AddOption(questionId, createOptionDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
           
        }

        [Authorize]
        [HttpDelete("{questionId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(int questionId, int optionId)
        {
            var response = await _questionService.DeleteOption(questionId, optionId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        // implement update?
    }
}
