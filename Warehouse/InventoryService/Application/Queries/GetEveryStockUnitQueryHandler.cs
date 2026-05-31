using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Queries
{

    internal record GetEveryStockUnitQuery(int ItemId,string? BarCode) : IRequest<Result<List<StockUnitGetResponse>>>;


    internal class GetEveryStockUnitQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<GetEveryStockUnitQuery, Result<List<StockUnitGetResponse>>>
    {

        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result<List<StockUnitGetResponse>>> Handle(GetEveryStockUnitQuery get_query, CancellationToken cancellationToken)
        {
            try
            {
                var dbQuery = dbContext.StockUnits.AsNoTracking().AsQueryable();

                dbQuery = dbQuery.Where(su => su.ItemId == get_query.ItemId);

                if (!string.IsNullOrWhiteSpace(get_query.BarCode))
                {
                    dbQuery = dbQuery.Where(su => su.SerialNumber.ToLower().Equals(get_query.BarCode.ToLower()));
                }

                var result = await dbQuery
                    .ProjectTo<StockUnitGetResponse>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<StockUnitGetResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<StockUnitGetResponse>>.Fail(ex.Message);
            }
        }
    }
}
