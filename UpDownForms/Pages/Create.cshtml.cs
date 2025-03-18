using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UpDownForms.Models;

namespace UpDownForms.Pages
{
    public class CreateModel : PageModel
    {
        private readonly UpDownFormsContext _context;

        public CreateModel(UpDownFormsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FormId"] = new SelectList(_context.Forms, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public Response Response { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Responses.Add(Response);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
