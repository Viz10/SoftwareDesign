using AutoMapper;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.DTOs.ItemDTO;
using InventoryService.Application.Export;
using AutoMapper.QueryableExtensions;

namespace InventoryService.Application.Queries
{
    internal record ExportItemsQuery(string Strategy) : IRequest<string>;


    internal class ExportItemsQueryHandler(InventoryServiceDbContext _dbContext, IMapper mapper) : IRequestHandler<ExportItemsQuery, string>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = mapper;

        public async Task<string> Handle(ExportItemsQuery query, CancellationToken ct) 
        {
            var items = await dbContext.Items.AsNoTracking().ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider).ToListAsync(ct);
            ExportStrategy export = new();
            var result = await export.ExportData(items, query.Strategy);
            return result;
        }
    }
}
