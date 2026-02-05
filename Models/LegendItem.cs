using System.ComponentModel.DataAnnotations;

namespace FlowChartApp.Models
{
    public class LegendItem
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        // CSS properties
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string BorderColor { get; set; } = "#000000";
        public string BorderStyle { get; set; } = "solid"; // solid, dashed, dotted
        
        public int OrderIndex { get; set; }
    }
}
