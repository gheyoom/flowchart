using System.ComponentModel.DataAnnotations;

namespace FlowChartApp.Models;

public class AppMetadata
{
    [Key]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
