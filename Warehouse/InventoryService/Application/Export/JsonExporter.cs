using System.Text.Json;

namespace InventoryService.Application.Export
{
    public class JsonExporter<T> : BaseExporter<T>
    {
        public override string StrategyName => "JSON";
        protected async override Task<string> Transform(List<T> data)
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
