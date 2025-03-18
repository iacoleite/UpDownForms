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
    public class IndexModel : PageModel
    {
        private readonly UpDownFormsContext _context;

        public IndexModel(UpDownFormsContext context)
        {
            _context = context;
        }

        public IList<Response> Response { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Response = await _context.Responses
                .Include(r => r.Form).ToListAsync();
        }
    }
}
