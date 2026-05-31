using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Queries
{
    internal record GetItemQuery(int Id) : IRequest<Result<ItemGetResponse>>;


    internal class GetItemQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<GetItemQuery, Result<ItemGetResponse>>
    {

        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result<ItemGetResponse>> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await dbContext.Items.AsNoTracking()
                    .Where(el => el.Id == request.Id)
                    .ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (item is null) return Result<ItemGetResponse>.Fail("Not Found");
                return Result<ItemGetResponse>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<ItemGetResponse>.Fail(ex.Message);
            }
        }
    }
}
