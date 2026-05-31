using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Application.DomainService;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Commands
{
    internal record UpdateItemCommand(int Id, string Name, decimal? ReferencePricePerItem, string? Description) : IRequest<Result<UpdateItemRequest>>;

    internal class UpdateItemCommandHandler(
        InventoryServiceDbContext _dbContext,
        IMapper _mapper,
        ItemDomainService itemDomainService,
        IHttpClientFactory httpFactory,
        CurrentUser user) : IRequestHandler<UpdateItemCommand, Result<UpdateItemRequest>>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;
        private readonly ItemDomainService _itemDomainService = itemDomainService;
        private readonly IHttpClientFactory _httpFactory = httpFactory;
        private readonly CurrentUser _user = user;

        public async Task<Result<UpdateItemRequest>> Handle(UpdateItemCommand command, CancellationToken ct)
        {
            try
            {
                var result_dup = await _itemDomainService.IsDuplicate(command.Name, command.Id);
                if (!result_dup.IsSuccessful) { return Result<UpdateItemRequest>.Fail(result_dup.Errors!["Error"].First()); }

                var Old = await dbContext.Items.FindAsync(command.Id);

                if (Old == null)
                {
                    return Result<UpdateItemRequest>.Fail("Not Found");
                }

                mapper.Map(command, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync(ct);

                var result = await dbContext.Items
                    .Where(x => x.Id == command.Id)
                    .ProjectTo<UpdateItemRequest>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);

                var client = _httpFactory.CreateClient("NotificationService");
                await client.PostAsJsonAsync("api/notify", new WarehouseEvent
                {
                    EntityType = "Item",
                    AccountEmail = _user.Email,
                    Action = "Updated",
                    Description = $"Item {command.Name} was updated",
                    OccurredAt = DateTimeOffset.UtcNow
                }, ct);

                return Result<UpdateItemRequest>.Success(result!);
            }
            catch (Exception ex)
            {
                return Result<UpdateItemRequest>.Fail(ex.Message);
            }
        }
    }
}