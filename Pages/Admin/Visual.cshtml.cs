using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;

using Microsoft.AspNetCore.Authorization;

namespace FlowChartApp.Pages.Admin;

[Authorize]
public class VisualModel : PageModel
{
    private readonly AppDbContext _context;

    public VisualModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<FlowBox> FlowBoxes { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.FlowBoxes != null)
        {
            FlowBoxes = await _context.FlowBoxes
                .Include(b => b.OutgoingConnections)
                .ThenInclude(c => c.TargetBox)
                .ToListAsync();
        }
    }
}
