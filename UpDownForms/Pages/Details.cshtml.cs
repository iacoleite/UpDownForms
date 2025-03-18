using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UpDownForms.Models;

namespace UpDownForms.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly UpDownFormsContext _context;

        public DetailsModel(UpDownFormsContext context)
        {
            _context = context;
        }

        public Response Response { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.Responses.FirstOrDefaultAsync(m => m.Id == id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                Response = response;
            }
            return Page();
        }
    }
}
