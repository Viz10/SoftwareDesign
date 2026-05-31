using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Commands
{
    internal record UpdateStockUnitCommand(int Id, int ItemId, decimal? CurrentPrice, string? Note, UnitStatus? Status) : IRequest<Result<UpdateStockUnitRequest>>;


    internal class UpdateStockUnitCommandHandler(InventoryServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<UpdateStockUnitCommand, Result<UpdateStockUnitRequest>>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result<UpdateStockUnitRequest>> Handle(UpdateStockUnitCommand command, CancellationToken ct)
        {
            try
            {
                var Old = await dbContext.StockUnits.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: ct);

                if (Old == null)
                {
                    return Result<UpdateStockUnitRequest>.Fail("Not Found");
                }

                mapper.Map(command, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync(ct);

                var result = await dbContext.StockUnits
                    .Where(x => x.Id == command.Id)
                    .ProjectTo<UpdateStockUnitRequest>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);

                return Result<UpdateStockUnitRequest>.Success(result!);
            }
            catch (DbUpdateException ex)
            {
                return Result<UpdateStockUnitRequest>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<UpdateStockUnitRequest>.Fail(ex.Message);
            }
        }
    }
}