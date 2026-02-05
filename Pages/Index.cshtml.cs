using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlowChartApp.Data;
using FlowChartApp.Models;

namespace FlowChartApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public string ConnectionJson { get; set; } = "[]";
    public List<FlowBox> FlowBoxes { get; set; } = new();
    public List<LegendItem> LegendItems { get; set; } = new();

    public async Task OnGetAsync()
    {
        if (_context.FlowBoxes != null)
        {
            FlowBoxes = await _context.FlowBoxes
                .Include(b => b.OutgoingConnections)
                .OrderBy(b => b.OrderIndex)
                .ToListAsync();

            LegendItems = await _context.LegendItems.OrderBy(x => x.OrderIndex).ToListAsync();

            var connections = FlowBoxes
                .SelectMany(b => b.OutgoingConnections)
                .Select(c => new 
                { 
                    id = c.Id,
                    source = $"box-{c.SourceBoxId}", 
                    target = $"box-{c.TargetBoxId}", 
                    style = c.ConnectionType.ToLower(), 
                    color = c.Color, 
                    direction = c.Direction,
                    labelIcon = c.LabelIcon,
                    waypoints = c.Waypoints
                })
                .ToList();

            ConnectionJson = System.Text.Json.JsonSerializer.Serialize(connections);
        }
    }
}
