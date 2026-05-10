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
    public record GetStockUnitQuery(int id) : IRequest<Result<StockUnitGetResponse>>;

    public class GetStockUnitQueryHandler : IRequestHandler<GetStockUnitQuery, Result<StockUnitGetResponse>>
    {

        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;

        public GetStockUnitQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<StockUnitGetResponse>> Handle(GetStockUnitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await dbContext.StockUnits.AsNoTracking().Where(el => el.Id == request.id).ProjectTo<StockUnitGetResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
                if (item == null) return Result<StockUnitGetResponse>.Fail("Not Found");
                return Result<StockUnitGetResponse>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<StockUnitGetResponse>.Fail(ex.Message);
            }
        }
    }

}
