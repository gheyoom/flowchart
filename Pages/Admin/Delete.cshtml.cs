using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;

using Microsoft.AspNetCore.Authorization;

namespace FlowChartApp.Pages.Admin;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public FlowBox FlowBox { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.FlowBoxes == null)
        {
            return NotFound();
        }

        var flowbox = await _context.FlowBoxes.FirstOrDefaultAsync(m => m.Id == id);

        if (flowbox == null)
        {
            return NotFound();
        }
        else 
        {
            FlowBox = flowbox;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.FlowBoxes == null)
        {
            return NotFound();
        }
        var flowbox = await _context.FlowBoxes.FindAsync(id);

        if (flowbox != null)
        {
            FlowBox = flowbox;
            _context.FlowBoxes.Remove(FlowBox);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
