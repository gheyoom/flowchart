using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;
using FlowChartApp.Models.Api;

namespace FlowChartApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlowController : ControllerBase
{
    private readonly AppDbContext _context;

    public FlowController(AppDbContext context)
    {
        _context = context;
    }

    private async Task UpdateTimestamp()
    {
        var meta = await _context.AppMetadata.FindAsync("LastUpdated");
        if (meta == null)
        {
            meta = new AppMetadata { Key = "LastUpdated" };
            _context.AppMetadata.Add(meta);
        }
        meta.Value = DateTime.Now.ToString("g"); // 'g' = General Date/Time pattern (short time)
        // No SaveChanges here, assume caller does it or we do it here?
        // Better to do it in the flow to avoid multiple DB calls if possible, but for simplicity let's rely on the caller's SaveChangesAsync if they are in same context scope.
        // Actually, change tracking handles it.
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveBox([FromBody] MoveBoxRequest request)
    {
        var box = await _context.FlowBoxes.FindAsync(request.Id);
        if (box == null) return NotFound();

        box.PosX = request.X;
        box.PosY = request.Y;
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateBox([FromBody] UpdateBoxRequest request)
    {
        var box = await _context.FlowBoxes.FindAsync(request.Id);
        if (box == null) return NotFound();

        box.Name = request.Name;
        box.Description = request.Description;
        box.DescriptionArabic = request.DescriptionArabic;
        box.LinkLeftText = request.LinkLeftText;
        box.LinkLeftUrl = request.LinkLeftUrl;
        box.LinkMiddleText = request.LinkMiddleText;
        box.LinkMiddleUrl = request.LinkMiddleUrl;
        box.LinkRightText = request.LinkRightText;
        box.LinkRightUrl = request.LinkRightUrl;
        
        box.BackgroundColor = request.BackgroundColor;
        box.BorderColor = request.BorderColor;
        box.BorderStyle = request.BorderStyle;
        
        box.Width = request.Width;
        box.Height = request.Height;
        
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteBox([FromBody] int id)
    {
        var box = await _context.FlowBoxes
            .Include(b => b.OutgoingConnections)
            .FirstOrDefaultAsync(b => b.Id == id);
            
        if (box == null) return NotFound();

        // Remove incoming connections to avoid orphans
        var incoming = await _context.FlowConnections.Where(c => c.TargetBoxId == id).ToListAsync();
        _context.FlowConnections.RemoveRange(incoming);
        
        _context.FlowBoxes.Remove(box);
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("connection/add")]
    public async Task<IActionResult> AddConnection([FromBody] AddConnectionRequest request)
    {
        var conn = new FlowConnection
        {
            SourceBoxId = request.SourceBoxId,
            TargetBoxId = request.TargetBoxId,
            ConnectionType = request.ConnectionType,
            Color = request.Color,
            Direction = request.Direction,
            LabelIcon = request.LabelIcon,
            Waypoints = request.Waypoints
        };
        _context.FlowConnections.Add(conn);
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok(conn.Id);
    }

    [HttpPost("connection/delete")]
    public async Task<IActionResult> DeleteConnection([FromQuery] int id)
    {
        var conn = await _context.FlowConnections.FindAsync(id);
        if (conn == null) return NotFound();
        
        _context.FlowConnections.Remove(conn);
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("connection/update")]
    public async Task<IActionResult> UpdateConnection([FromBody] UpdateConnectionRequest request)
    {
        var conn = await _context.FlowConnections.FindAsync(request.Id);
        if (conn == null) return NotFound();

        conn.ConnectionType = request.ConnectionType;
        conn.Color = request.Color;
        conn.Direction = request.Direction;
        conn.LabelIcon = request.LabelIcon;
        conn.Waypoints = request.Waypoints;

        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }

    // --- Legend API ---

    [HttpGet("legend")]
    public async Task<IActionResult> GetLegend()
    {
        var items = await _context.LegendItems.OrderBy(x => x.OrderIndex).ToListAsync();
        return Ok(items);
    }

    [HttpPost("legend/add")]
    public async Task<IActionResult> AddLegendItem([FromBody] LegendItem item)
    {
        // Auto order
        int maxOrder = await _context.LegendItems.MaxAsync(x => (int?)x.OrderIndex) ?? 0;
        item.OrderIndex = maxOrder + 1;
        
        _context.LegendItems.Add(item);
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok(item.Id);
    }

    [HttpPost("legend/update")]
    public async Task<IActionResult> UpdateLegendItem([FromBody] LegendItem item)
    {
        var existing = await _context.LegendItems.FindAsync(item.Id);
        if (existing == null) return NotFound();
        
        existing.Title = item.Title;
        existing.BackgroundColor = item.BackgroundColor;
        existing.BorderColor = item.BorderColor;
        existing.BorderStyle = item.BorderStyle;
        
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("legend/delete")]
    public async Task<IActionResult> DeleteLegendItem([FromBody] int id)
    {
        var item = await _context.LegendItems.FindAsync(id);
        if (item == null) return NotFound();
        
        _context.LegendItems.Remove(item);
        await UpdateTimestamp();
        await _context.SaveChangesAsync();
        return Ok();
    }
}
