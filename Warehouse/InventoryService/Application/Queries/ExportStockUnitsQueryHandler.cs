using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Application.Export;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Queries
{
    internal record ExportStockUnitsQuery(string Strategy, int ItemId) : IRequest<string>;


    internal class ExportStockUnitsQueryHandler(InventoryServiceDbContext _dbContext, IMapper mapper) : IRequestHandler<ExportStockUnitsQuery, string>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = mapper;

        public async Task<string> Handle(ExportStockUnitsQuery query, CancellationToken ct)
        {
            var su = await dbContext.StockUnits.AsNoTracking()
                .Where(su=>su.ItemId==query.ItemId)
                .ProjectTo<StockUnitGetResponse>(mapper.ConfigurationProvider).ToListAsync(ct);
                
            ExportStrategy export = new();
            var result = await export.ExportData(su, query.Strategy);
            return result;
        }
    }
}
