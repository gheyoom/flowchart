using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FlowChartApp.Data;
using FlowChartApp.Models;

using Microsoft.AspNetCore.Authorization;

namespace FlowChartApp.Pages.Admin;

[Authorize]
public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public FlowBox FlowBox { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.FlowBoxes.Add(FlowBox);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
