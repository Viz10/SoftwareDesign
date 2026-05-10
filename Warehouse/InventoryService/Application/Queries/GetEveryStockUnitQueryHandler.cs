using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Queries
{

    public record GetEveryStockUnitQuery(string? barCode) : IRequest<Result<List<StockUnitGetResponse>>>;

    public class GetEveryStockUnitQueryHandler : IRequestHandler<GetEveryStockUnitQuery, Result<List<StockUnitGetResponse>>>
    {

        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;

        public GetEveryStockUnitQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<List<StockUnitGetResponse>>> Handle(GetEveryStockUnitQuery get_query, CancellationToken cancellationToken)
        {
            try
            {
                var dbQuery = dbContext.StockUnits.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(get_query.barCode))
                {
                    dbQuery = dbQuery.Where(item => item.SerialNumber.ToLower().Equals(get_query.barCode.ToLower()));
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
