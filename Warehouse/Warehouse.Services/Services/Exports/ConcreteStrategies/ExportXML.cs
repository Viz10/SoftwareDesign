using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Warehouse.Services.Services.Exports.ConcreteStrategies
{
    public class ExportXML : IExportStrategy
    {
        public string StrategyName => "XML";
        public async Task<string> ExportData<T>(List<T> data)
        {
            return await Task.Run(async () =>
            {
                var serializer = new XmlSerializer(data.GetType());
                using var sw = new StringWriter();
                serializer.Serialize(sw, data);
                return sw.ToString();
            });
        }
    }
}
