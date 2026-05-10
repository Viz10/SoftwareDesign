using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;
using Warehouse.Shared.DTOs.StockUnitDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventoryService.Application.Commands
{
    public record UpdateStockUnitCommand(int Id, int ItemId, decimal? CurrentPrice, string? Note, UnitStatus? Status) : IRequest<Result<StockUnitGetResponse>>;

    internal class UpdateStockUnitCommandHandler : IRequestHandler<UpdateStockUnitCommand, Result<StockUnitGetResponse>>
    {
        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;

        public UpdateStockUnitCommandHandler(InventoryServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<StockUnitGetResponse>> Handle(UpdateStockUnitCommand command, CancellationToken ct)
        {
            try
            {
                var Old = await dbContext.StockUnits.FirstOrDefaultAsync(x => x.Id == command.Id);

                if (Old == null)
                {
                    return Result<StockUnitGetResponse>.Fail("Not Found");
                }

                mapper.Map(command, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync(ct);

                var result = await dbContext.StockUnits
                    .Where(x => x.Id == command.Id)
                    .ProjectTo<StockUnitGetResponse>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);

                return Result<StockUnitGetResponse>.Success(result);
            }
            catch (DbUpdateException ex)
            {
                return Result<StockUnitGetResponse>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<StockUnitGetResponse>.Fail(ex.Message);
            }
        }
    }


}
