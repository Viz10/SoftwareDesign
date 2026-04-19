using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Services.Services.Exports.ConcreteStrategies
{
    public class ExportCSV : IExportStrategy
    {
        public string StrategyName => "CSV";
        public async Task<string> ExportData<T>(List<T> data)
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
