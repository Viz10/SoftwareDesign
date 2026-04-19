using Microsoft.AspNetCore.Components;

namespace Warehouse.Web.Models
{
    public class GenericTablePlugIns
    {
        public string? TableTitle { get; set; }
        public List<string> ColumnNames { get; set; } = null!;
        public EventCallback<int> OnDeleteCallback { get; set; } /// delegate delete functionality to parent
        public string EditBasePagePath { get; set; } = null!; /// redirect to edit page
        public string AddPagePath { get; set; } = null!; /// redirect to add page
        public List<(string ButtonLabel, EventCallback<int> Action)>? ExtraActions { get; set; } /// set of optional buttons with custom action
    }
}
