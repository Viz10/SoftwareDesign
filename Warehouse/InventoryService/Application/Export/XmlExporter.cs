using System.Xml.Serialization;

namespace InventoryService.Application.Export
{
    public class XmlExporter<T> : BaseExporter<T>
    {
        public override string StrategyName => "XML";
        protected async override Task<string> Transform(List<T> data)
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
