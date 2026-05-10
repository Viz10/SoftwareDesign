using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record AddStockUnitRequest(int ItemId, decimal? CurrentPrice, string? Note, int Quantity);
}
