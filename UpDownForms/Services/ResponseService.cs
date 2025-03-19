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

        public async Task<ApiResponse<IEnumerable<ResponseDTO>>> GetResponses()
        {
            var response = await _context.Responses
                   .Include(r => r.Form)
                   .ThenInclude(f => f.User)
                   .Include(r => r.Answers)
                   .Where(r => !r.IsDeleted)
                   .ToListAsync();
            if (response == null)
            {
                return new ApiResponse<IEnumerable<ResponseDTO>>(false, "Can't find any response", null);
            }

            return new ApiResponse<IEnumerable<ResponseDTO>>(true, "OK", response.Select(r => r.ToResponseDTO()).ToList());
        }

        public async Task<ApiResponse<ResponseFormNoResponseDTO>> GetResponseById(int id)
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
                return new ApiResponse<ResponseFormNoResponseDTO>(false, "Can't find  response", null);
            }
            return new ApiResponse<ResponseFormNoResponseDTO>(true, "Ok", response.ToResponseFormNoResponseDTO());
        }

        public async Task<ApiResponse<ResponseDTO>> PostResponse(CreateResponseDTO createResponseDTO)
        {
            if (createResponseDTO == null)
            {
                return new ApiResponse<ResponseDTO>(false, "Missing response data", null);
            }
            var formExists = await _context.Forms.AnyAsync(f => f.Id == createResponseDTO.FormId);
            if (!formExists)
            {
                return new ApiResponse<ResponseDTO>(false, "Form does not exists", null);
            }
            var response = new Response(createResponseDTO);
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();

            return new ApiResponse<ResponseDTO>(true, "Ok", response.ToResponseDTO());
        }

        public async Task<ApiResponse<ResponseDTO>> DeleteResponse(int id)
        {
            var response = await _context.Responses.Include(r => r.Answers).FirstOrDefaultAsync(r => id == r.Id);
            if (response == null)
            {
                return new ApiResponse<ResponseDTO>(false, "Missing response data", null);
            }
            response.DeleteResponse();
            //response.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new ApiResponse<ResponseDTO>(true, "Ok", response.ToResponseDTO());
        }

        public async Task<ApiResponse<AnswerDTO>> PostAnswer(int id, CreateAnswerDTO createAnswerDTO)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                return new ApiResponse<AnswerDTO>(false, "Can't find Response", null);
            }
            var form = await _context.Forms.FindAsync(response.FormId);
            if (form == null)
            {
                return new ApiResponse<AnswerDTO>(false, "Can't find Form", null);
            }
            var question = await _context.Questions.FindAsync(createAnswerDTO.QuestionId);
            if (question == null)
            {
                return new ApiResponse<AnswerDTO>(false, "Can't find Question", null);
            }
            if (question.Id != createAnswerDTO.QuestionId)
            {
                return new ApiResponse<AnswerDTO>(false, "QuestionId does not match the question in the form", null);
            }
            if (form.Id != question.FormId)
            {
                return new ApiResponse<AnswerDTO>(false, "FormId does not match the form of the question", null);

            }
            if (question.GetType().Name != ("Question" + createAnswerDTO.Type))
            {
                return new ApiResponse<AnswerDTO>(false, "Answer type does not match the question type", null);
            }

            if (createAnswerDTO is CreateAnswerOpenEndedDTO answerOpenEndedDTO)
            {
                var answer = new AnswerOpenEnded(answerOpenEndedDTO);
                answer.AnswerText = answerOpenEndedDTO.AnswerText;
                answer.ResponseId = id;
                answer.QuestionId = createAnswerDTO.QuestionId;

                await _context.Answers.AddAsync(answer);
                await _context.SaveChangesAsync();

                return new ApiResponse<AnswerDTO>(true, "OK", answer.ToAnswerOpenEndedResponseDTO());

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
                    return new ApiResponse<AnswerDTO>(true, "OK", answer.ToAnswerMultipleChoiceResponseDTO());
                }
                catch (Exception e)
                {
                    return new ApiResponse<AnswerDTO>(false, "Something went wrong!", null);
                }
            }
            else
            {
                return new ApiResponse<AnswerDTO>(false, "Invalid Answer Type", null);
            }
        }
    }
}
