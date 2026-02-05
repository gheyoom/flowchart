using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;

using Microsoft.AspNetCore.Authorization;

namespace FlowChartApp.Pages.Admin;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<FlowBox> FlowBox { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.FlowBoxes != null)
        {
            FlowBox = await _context.FlowBoxes.OrderBy(b => b.OrderIndex).ToListAsync();
        }
    }
}
