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

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestionById(int id)
        {

            var response = await _questionService.GetQuestionById(id);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionDetailsDTO>> PostQuestion([FromBody] CreateQuestionDTO createQuestionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _questionService.PostQuestion(createQuestionDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDTO>> PutQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _questionService.PutQuestion(id, updateQuestionDTO);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDTO>> DeleteQuestion(int id)
        {
            var response = await _questionService.DeleteQuestion(id);
            return Ok(response);
        
        }


        // Handling options
        [HttpGet("{questionId}/options")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetOptions(int questionId)
        {
            var response = await _questionService.GetOptionsByQuestion(questionId);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("{questionId}/options")]
        public async Task<IActionResult> AddOption(int questionId, [FromBody] CreateOptionDTO createOptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _questionService.AddOption(questionId, createOptionDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{questionId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOption(int questionId, int optionId)
        {
            var response = await _questionService.DeleteOption(questionId, optionId);
            return Ok(response);
        }
    

        // implement update?
    }
}
