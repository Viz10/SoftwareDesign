namespace Warehouse.Models
{
    public class GenericTableModel
    {
        public string Controller { get; set; } = null!;
        public List<string> ColumnNames { get; set; } = null!;
        public IEnumerable<Dictionary<string, object>> RowPairs { get; set; }  = null!;
        public string title { get; set; } = null!;
        public List<(string Label, string Action, string? NewController)>? ExtraActions { get; set; }
    }
}
