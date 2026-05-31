using System.Text;

namespace InventoryService.Application.Export
{
    public class CsvExporter<T> : BaseExporter<T>
    {
        public override string StrategyName => "CSV";
        protected override async Task<string> Transform(List<T> data)
        {
            return await Task.Run(async () =>
            {

                var Type = typeof(T);
                var sb = new StringBuilder();

                var Properties = Type.GetProperties();
                sb.AppendLine(string.Join(", ", Properties.Select(prop => prop.Name)));

                foreach (var item in data)
                {
                    var line = Properties.Select(p =>
                    {
                        var value = p.GetValue(item)?.ToString() ?? "";
                        return $"\"{value.Replace("\"", "\"\"")}\"";
                    });
                    sb.AppendLine(string.Join(",", line));
                }

                return sb.ToString();
            });
        }
    }
}
