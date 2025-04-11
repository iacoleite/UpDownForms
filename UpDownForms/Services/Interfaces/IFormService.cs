using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.ResponseDTOs;
using UpDownForms.Pagination;

namespace UpDownForms.Services.Interfaces
{
    public interface IFormService
    {
        Task<Pageable<FormDTO>> GetForms(PageParameters pageParameters);
        Task<FormDTO> GetForm(int id);
        Task<FormDTO> PostForm(CreateFormDTO createFormDTO);
        Task<FormDTO> PutForm(int id, UpdateFormDTO updateFormDTO);
        Task<FormDTO> DeleteForm(int id);
        Task<Pageable<ResponseDTO>> GetResponsesByFormId(int id, PageParameters pageParameters);
    }
}
