using AutoMapper;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;
using static InventoryService.Application.Queries.ExportQueryHandler;
using InventoryService.Application.Export;
using AutoMapper.QueryableExtensions;

namespace InventoryService.Application.Queries
{
    public record ExportItemsQuery(string strategy) : IRequest<string>;
    
    public class ExportQueryHandler : IRequestHandler<ExportItemsQuery, string>
    {
        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;
        public ExportQueryHandler(InventoryServiceDbContext _dbContext, IMapper mapper)
        {
            dbContext = _dbContext;
            this.mapper = mapper;
        }
        public async Task<string> Handle(ExportItemsQuery query, CancellationToken ct) 
        {
            var items = await dbContext.Items.AsNoTracking().ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider).ToListAsync(ct);
            ExportStrategy export = new ExportStrategy();
            var result = await export.ExportData(items, query.strategy);
            return result;
        }
    }
}
