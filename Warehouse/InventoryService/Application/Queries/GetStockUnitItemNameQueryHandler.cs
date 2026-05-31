using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;


namespace InventoryService.Application.Queries
{

    internal record GetStockUnitItemNameQuery(int Id) : IRequest<Result<string>>;


    internal class GetStockUnitItemNameQueryHandler(InventoryServiceDbContext _dbContext) : IRequestHandler<GetStockUnitItemNameQuery, Result<string>>
    {

        private readonly InventoryServiceDbContext dbContext = _dbContext;

        public async Task<Result<string>> Handle(GetStockUnitItemNameQuery request, CancellationToken ct)
        {
            try
            {
                var name = await dbContext.Items
                .Where(i => i.Id == request.Id)
                .Select(i => i.Name).FirstOrDefaultAsync(ct);

                return name is null
                    ? Result<string>.Fail("Item name not found") 
                    : Result<string>.Success(name);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }
    }
}
