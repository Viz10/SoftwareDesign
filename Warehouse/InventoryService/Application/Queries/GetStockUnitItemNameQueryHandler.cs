using AutoMapper;
using Azure.Core;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Queries
{

    public record GetStockUnitItemNameQuery(int id) : IRequest<Result<string>>;

    public class GetStockUnitItemNameQueryHandler : IRequestHandler<GetStockUnitItemNameQuery, Result<string>>
    {

        private readonly InventoryServiceDbContext dbContext;

        public GetStockUnitItemNameQueryHandler(InventoryServiceDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Result<string>> Handle(GetStockUnitItemNameQuery request, CancellationToken ct)
        {
            try
            {
                var name = await dbContext.Items
                .Where(i => i.Id == request.id)
                .Select(i => i.Name).FirstOrDefaultAsync(ct);

                return name != null
                    ? Result<string>.Success(name)
                    : Result<string>.Fail("Item name not found");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }
    }

}
