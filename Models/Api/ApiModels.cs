namespace FlowChartApp.Models.Api;

public class MoveBoxRequest
{
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}

public class UpdateBoxRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public string? LinkLeftText { get; set; }
    public string? LinkLeftUrl { get; set; }
    public string? LinkMiddleText { get; set; }
    public string? LinkMiddleUrl { get; set; }
    public string? LinkRightText { get; set; }
    public string? LinkRightUrl { get; set; }
    
    public string BackgroundColor { get; set; } = "#ffffff";
    public string BorderColor { get; set; } = "#6c757d";
    public string BorderStyle { get; set; } = "solid";
    
    public string? Width { get; set; }
    public string? Height { get; set; }
}

public class AddConnectionRequest
{
    public int SourceBoxId { get; set; }
    public int TargetBoxId { get; set; }
    public string ConnectionType { get; set; } = "solid";
    public string Color { get; set; } = "black";
    public string Direction { get; set; } = "right";
    public string? LabelIcon { get; set; }
    public string? Waypoints { get; set; }
}

public class UpdateConnectionRequest
{
    public int Id { get; set; }
    public string ConnectionType { get; set; }
    public string Color { get; set; }
    public string Direction { get; set; }
    public string? LabelIcon { get; set; }
    public string? Waypoints { get; set; }
}
