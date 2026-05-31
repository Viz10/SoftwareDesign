using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Queries
{
    internal record GetStockUnitQuery(int Id) : IRequest<Result<StockUnitGetResponse>>;


    internal class GetStockUnitQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<GetStockUnitQuery, Result<StockUnitGetResponse>>
    {

        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result<StockUnitGetResponse>> Handle(GetStockUnitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await dbContext.StockUnits.AsNoTracking()
                    .Where(el => el.Id == request.Id)
                    .ProjectTo<StockUnitGetResponse>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                
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
