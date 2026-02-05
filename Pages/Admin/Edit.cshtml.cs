using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;

using Microsoft.AspNetCore.Authorization;

namespace FlowChartApp.Pages.Admin;

[Authorize]
public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public FlowBox FlowBox { get; set; } = default!;

    // For adding a new connection
    [BindProperty]
    public FlowConnection NewConnection { get; set; } = new();

    public SelectList BoxList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.FlowBoxes == null)
        {
            return NotFound();
        }

        var flowbox =  await _context.FlowBoxes
            .Include(b => b.OutgoingConnections)
            .ThenInclude(c => c.TargetBox)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (flowbox == null)
        {
            return NotFound();
        }
        FlowBox = flowbox;
        
        // Populate dropdown excludes current box
        var otherBoxes = await _context.FlowBoxes.Where(b => b.Id != id).ToListAsync();
        BoxList = new SelectList(otherBoxes, "Id", "Name");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Simple update of the main box details
        var boxToUpdate = await _context.FlowBoxes.FindAsync(FlowBox.Id);
        if (boxToUpdate == null) return NotFound();

        boxToUpdate.Name = FlowBox.Name;
        boxToUpdate.OrderIndex = FlowBox.OrderIndex;
        boxToUpdate.LinkLeftText = FlowBox.LinkLeftText;
        boxToUpdate.LinkLeftUrl = FlowBox.LinkLeftUrl;
        boxToUpdate.LinkMiddleText = FlowBox.LinkMiddleText;
        boxToUpdate.LinkMiddleUrl = FlowBox.LinkMiddleUrl;
        boxToUpdate.LinkRightText = FlowBox.LinkRightText;
        boxToUpdate.LinkRightUrl = FlowBox.LinkRightUrl;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostAddConnectionAsync()
    {
        // Manually bind the SourceBoxId to the current page's box
        NewConnection.SourceBoxId = FlowBox.Id;

        _context.FlowConnections.Add(NewConnection);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Edit", new { id = FlowBox.Id });
    }

    public async Task<IActionResult> OnPostDeleteConnectionAsync(int connectionId)
    {
        var conn = await _context.FlowConnections.FindAsync(connectionId);
        if (conn != null)
        {
            _context.FlowConnections.Remove(conn);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Edit", new { id = FlowBox.Id }); 
    }
}
