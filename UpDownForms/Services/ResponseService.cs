using Microsoft.EntityFrameworkCore;
using System.Net;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Models;

namespace UpDownForms.Services
{
    public class ResponseService
    {
        private readonly UpDownFormsContext _context;
        private readonly IUserService _userService;

        public ResponseService(UpDownFormsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<ResponseDTO>> GetResponses()
        {
            var response = await _context.Responses
                   .Include(r => r.Form)
                   .ThenInclude(f => f.User)
                   .Include(r => r.Answers)
                   .Where(r => !r.IsDeleted)
                   .ToListAsync();
            if (response == null)
            {
                throw new EntityNotFoundException("Can't find response");
            }

            return response.Select(r => r.ToResponseDTO()).ToList();
        }

        public async Task<ResponseFormNoResponseDTO> GetResponseById(int id)
        {
            var response = await _context.Responses
                .Include(r => r.Form)
                .ThenInclude(f => f.User)
                .Include(r => r.Answers)
                //.Include(r => r.Form)
                //.ThenInclude(f => f.Questions)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (response == null)
            {
                throw new EntityNotFoundException("Can't find response");
            }
            return response.ToResponseFormNoResponseDTO();
        }

        public async Task<ResponseDTO> PostResponse(CreateResponseDTO createResponseDTO)
        {
            if (createResponseDTO == null)
            {
                throw new BadHttpRequestException("Missing input data");
            }
            var formExists = await _context.Forms.AnyAsync(f => f.Id == createResponseDTO.FormId);
            if (!formExists)
            {
                throw new EntityNotFoundException("Can't find Form");
            }
            var response = new Response(createResponseDTO);
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();

            return response.ToResponseDTO();
        }

        public async Task<ResponseDTO> DeleteResponse(int id)
        {
            var response = await _context.Responses.Include(r => r.Answers).FirstOrDefaultAsync(r => id == r.Id);
            if (response == null)
            {
                throw new EntityNotFoundException("Can't find Form");
            }
            response.DeleteResponse();
            //response.IsDeleted = true;
            await _context.SaveChangesAsync();
            return response.ToResponseDTO();
        }

        public async Task<AnswerDTO> PostAnswer(int id, CreateAnswerDTO createAnswerDTO)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                throw new EntityNotFoundException("Can't find Response");
            }
            var form = await _context.Forms.FindAsync(response.FormId);
            if (form == null)
            {
                throw new EntityNotFoundException("Can't find Form");
            }
            var question = await _context.Questions.FindAsync(createAnswerDTO.QuestionId);
            if (question == null)
            {
                throw new EntityNotFoundException("Can't find Question");
            }
            if (question.Id != createAnswerDTO.QuestionId)
            {
                throw new BadHttpRequestException("QuestionId does not match the question in the form");
            }
            if (form.Id != question.FormId)
            {
                throw new BadHttpRequestException("FormId does not match the form of the question");
            }

            if (question.GetType().Name != ("Question" + createAnswerDTO.Type))
            {
                throw new BadHttpRequestException("Answer type does not match the question type");
            }

            var existingAnswer = await _context.Answers.FirstOrDefaultAsync(a => a.ResponseId == id && a.QuestionId == createAnswerDTO.QuestionId);
            if (existingAnswer != null)
            {
                throw new BadHttpRequestException("Response already has an answer for this question");
            }

            if (createAnswerDTO is CreateAnswerOpenEndedDTO answerOpenEndedDTO)
            {
                var answer = new AnswerOpenEnded(answerOpenEndedDTO);
                answer.AnswerText = answerOpenEndedDTO.AnswerText;
                answer.ResponseId = id;
                answer.QuestionId = createAnswerDTO.QuestionId;

                await _context.Answers.AddAsync(answer);
                await _context.SaveChangesAsync();

                return answer.ToAnswerOpenEndedResponseDTO();
            }
            else if (createAnswerDTO is CreateAnswerMultipleChoiceDTO createAnswerMultipleChoiceDTO)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var answer = new AnswerMultipleChoice(createAnswerMultipleChoiceDTO);
                    answer.ResponseId = id;
                    answer.QuestionId = createAnswerMultipleChoiceDTO.QuestionId;

                    await _context.AnswersMultipleChoice.AddAsync(answer);
                    await _context.SaveChangesAsync();

                    foreach (var optionId in createAnswerMultipleChoiceDTO.SelectedOptions.Distinct())
                    {
                        var existingAnsweredOption = await _context.AnsweredOptions.FirstOrDefaultAsync(ao => ao.OptionId == optionId && ao.AnswerMultipleChoiceId == answer.Id);
                        if (existingAnsweredOption == null)
                        {
                            var answeredOption = new AnsweredOption
                            {
                                AnswerMultipleChoiceId = answer.Id,
                                OptionId = optionId
                            };
                            await _context.AnsweredOptions.AddAsync(answeredOption);
                        }
                    }
                    await transaction.CommitAsync();
                    var answerMultipleChoiceResponseDTO = answer.ToAnswerMultipleChoiceResponseDTO();
                    foreach ( var selectedOption in answer.SelectedOptions)
                    {
                        answerMultipleChoiceResponseDTO.SelectedOptions.Add(selectedOption.OptionId);
                    }
                    return answerMultipleChoiceResponseDTO;
                }
                catch (Exception e)
                {
                    throw new Exception("Something went wrong!");
                }
            }
            else
            {
                throw new BadHttpRequestException("Invalid Answer Type");
            }
        }
    }
}
