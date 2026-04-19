using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Services.Services.Exports
{
    public class Export
    {
        public Dictionary<string, IExportStrategy> StrategyMap { get; set; }

        public Export(IEnumerable<IExportStrategy> AllStrategies)
        {
            StrategyMap = AllStrategies.ToDictionary(strat => strat.StrategyName, strat => strat);
        }

        public async Task<string> ExportData<T>(List<T> data, string StrategyName)
        {
            return await StrategyMap[StrategyName].ExportData<T>(data);
        }
    }
}
