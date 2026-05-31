namespace InventoryService.Application.Export
{
    public abstract class BaseExporter<T>
    {
        public abstract string StrategyName { get; }

        public async Task<string> Export(List<T> data) /// gets raw data => formated string
        {
            var transformed = await Transform(data);  
            return transformed;    
        }

        protected abstract Task<string> Transform(List<T> data); /// to be implemented by children class
    }
}
