using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Services.Services.Exports
{
    public interface IExportStrategy
    {
        string StrategyName { get; }
        Task<string> ExportData<T>(List<T> data);
    }
}
