using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record StockUnitGetResponse(
     int Id,
     string Name,
     string SerialNumber,
     string? Note,
     decimal? CurrentPrice,
     UnitStatus Status
    );

}
