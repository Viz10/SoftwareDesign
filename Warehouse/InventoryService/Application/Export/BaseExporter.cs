namespace InventoryService.Application.Export
{
    public abstract class BaseExporter<T>
    {
        public abstract string StrategyName { get; }

        public async Task<string> Export(List<T> data)
        {
            var transformed = await Transform(data);  
            return WriteOutput(transformed);    
        }

        protected abstract Task<string> Transform(List<T> data);

        private string WriteOutput(string content)
        {
            return content;
        }
    }
}
