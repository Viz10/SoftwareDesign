using AutoMapper;
using InventoryService.Application.DomainService;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;

namespace InventoryService.Application.Commands
{
    internal record AddItemCommand(string Name, decimal? ReferencePricePerItem, string? Description) : IRequest<Result<MessageResponse>>;


    internal class AddItemCommandHandler(
        InventoryServiceDbContext _dbContext,
        IMapper _mapper,
        ItemDomainService itemDomainService,
        IHttpClientFactory httpFactory,
        CurrentUser user) : IRequestHandler<AddItemCommand, Result<MessageResponse>>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;
        private readonly ItemDomainService _itemDomainService = itemDomainService;
        private readonly IHttpClientFactory _httpFactory = httpFactory;
        private readonly CurrentUser _user = user;

        public async Task<Result<MessageResponse>> Handle(AddItemCommand command, CancellationToken ct)
        {
            try
            {
                var result_dup = await _itemDomainService.IsDuplicate(command.Name, null);
                if (!result_dup.IsSuccessful) return Result<MessageResponse>.Fail(result_dup.Errors!["Error"].First());

                var result_restored = await _itemDomainService.Restore(command);
                if (!result_restored.IsSuccessful) return Result<MessageResponse>.Fail(result_restored.Errors!["Error"].First());

                if (result_restored.Value)/// restored
                {
                    await dbContext.SaveChangesAsync(ct);
                }
                else
                {
                    /// add new
                    var toBeAdded = mapper.Map<Item>(command);
                    await dbContext.Items.AddAsync(toBeAdded, ct);
                    await dbContext.SaveChangesAsync(ct);
                }

                var client = _httpFactory.CreateClient("NotificationService");
                await client.PostAsJsonAsync("api/notify", new WarehouseEvent
                {
                    EntityType = "Item",
                    AccountEmail = _user.Email,
                    Action = "Created",
                    Description = $"Item {command.Name} was created",
                    OccurredAt = DateTimeOffset.UtcNow
                }, ct);

                return Result<MessageResponse>.Success(MessageResponse.CreateMessage("Succesfully added new item!"));
            }
            catch (DbUpdateException ex)
            {
                return Result<MessageResponse>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<MessageResponse>.Fail(ex.Message);
            }
        }
    }
}