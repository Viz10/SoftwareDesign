using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record UpdateStockUnitRequest(int Id,int ItemId, decimal? CurrentPrice, string? Note, UnitStatus? Status);
}
