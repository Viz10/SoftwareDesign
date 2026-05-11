namespace InventoryService.Application.Export
{
    public class ExportStrategy
    {
        public async Task<string> ExportData<T>(List<T> data, string strategyName)
        {
            BaseExporter<T> exporter = strategyName.ToUpper() switch
            {
                "CSV" => new CsvExporter<T>(),
                "JSON" => new JsonExporter<T>(),
                "XML" => new XmlExporter<T>(),
                _ => throw new NotImplementedException(),
            };

            return await exporter.Export(data);
        }
    }
}
