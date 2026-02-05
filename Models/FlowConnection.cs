using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowChartApp.Models;

public class FlowConnection
{
    public int Id { get; set; }

    public int SourceBoxId { get; set; }
    public FlowBox? SourceBox { get; set; }

    public int TargetBoxId { get; set; }
    [ForeignKey("TargetBoxId")]
    public FlowBox? TargetBox { get; set; }

    public string ConnectionType { get; set; } = "Solid"; // Solid, Dotted
    public string Color { get; set; } = "Black"; // red, blue, etc
    public string Direction { get; set; } = "Right"; // From Source to Target (visual hint)
    public string? LabelIcon { get; set; }
    public string? Waypoints { get; set; } // JSON array of {x, y} coordinates
}
