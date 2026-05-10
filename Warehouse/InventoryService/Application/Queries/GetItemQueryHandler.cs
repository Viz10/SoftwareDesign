using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Queries
{
    public record GetItemQuery(int id) : IRequest<Result<ItemGetResponse>>;

    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, Result<ItemGetResponse>>
    {

        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;

        public GetItemQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<ItemGetResponse>> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await dbContext.Items.AsNoTracking().Where(el => el.Id == request.id).ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
                if (item == null) return Result<ItemGetResponse>.Fail("Not Found");
                return Result<ItemGetResponse>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<ItemGetResponse>.Fail(ex.Message);
            }
        }
    }
}
