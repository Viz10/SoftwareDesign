using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Warehouse.Services.Services.Exports.ConcreteStrategies
{
    public class ExportJSON :IExportStrategy
    {
        public string StrategyName => "JSON";

        public async Task<string> ExportData<T>(List<T> data)
        {
            return await Task.Run(async () =>
            {
                var context = new JsonSerializerOptions();
                context.WriteIndented = true;
                return JsonSerializer.Serialize(data, context);
            });
        }
    }
}
