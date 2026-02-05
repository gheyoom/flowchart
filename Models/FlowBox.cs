using System.ComponentModel.DataAnnotations;

namespace FlowChartApp.Models;

public class FlowBox
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }

    public string? LinkLeftUrl { get; set; }
    public string? LinkLeftText { get; set; }

    public string? LinkMiddleUrl { get; set; }
    public string? LinkMiddleText { get; set; }

    public string? LinkRightUrl { get; set; }
    public string? LinkRightText { get; set; }

    public int OrderIndex { get; set; }

    public double PosX { get; set; } = 100;
    public double PosY { get; set; } = 100;

    public string? Width { get; set; }
    public string? Height { get; set; }

    public string BackgroundColor { get; set; } = "#ffffff";
    public string BorderColor { get; set; } = "#6c757d";
    public string BorderStyle { get; set; } = "solid";

    public List<FlowConnection> OutgoingConnections { get; set; } = new();
}
