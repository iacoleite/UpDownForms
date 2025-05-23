﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
using UpDownForms.Services;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : Controller
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet]
        public async Task<ActionResult<Pageable<ResponseDTO>>> GetResponses([FromQuery] PageParameters pageParameters)
        {
            var response = await _responseService.GetResponses(pageParameters);
            this.AddPaginationMetadata(response, pageParameters);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseFormNoResponseDTO>> GetResponse(int id)
        {
            var response = await _responseService.GetResponseById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> PostResponse([FromBody] CreateResponseDTO createResponseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _responseService.PostResponse(createResponseDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResponse(int id)
        {
            var response = await _responseService.DeleteResponse(id);
            return Ok(response);
        }

        [HttpPost("{id}/answers/")]
        public async Task<ActionResult<AnswerDTO>> PostAnswer(int id, [FromBody] CreateAnswerDTO createAnswerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _responseService.PostAnswer(id, createAnswerDTO);
            return Ok(response);
        }
    }
}

