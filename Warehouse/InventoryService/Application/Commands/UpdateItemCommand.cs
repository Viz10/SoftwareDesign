using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using InventoryService.Application.DomainService;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Globalization;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryService.Application.Commands
{
    public record UpdateItemCommand(int Id, string Name, decimal? ReferencePricePerItem, string? Description) : IRequest<Result<ItemGetResponse>>;

    internal class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<ItemGetResponse>>
    {
        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ItemDomainService _itemDomainService;
        private readonly IHttpClientFactory _httpFactory;
        private readonly CurrentUser _user;

        public UpdateItemCommandHandler(
            InventoryServiceDbContext _dbContext,
            IMapper _mapper,
            ItemDomainService itemDomainService,
            IHttpClientFactory httpFactory,
            CurrentUser user)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            _itemDomainService = itemDomainService;
            _httpFactory = httpFactory;
            _user = user;
        }

        public async Task<Result<ItemGetResponse>> Handle(UpdateItemCommand command, CancellationToken ct)
        {
            /*
            try
            {
                var result_dup = await _itemDomainService.isDuplicate(command.Name, command.Id);
                if (!result_dup.IsSuccessful) { return Result<ItemGetResponse>.Fail(result_dup.ErrorMsg); }
                if (result_dup.Value) { return Result<ItemGetResponse>.Fail("Duplicate Item"); }

                var Old = await dbContext.Items.FindAsync(command.Id);

                if (Old == null)
                {
                    return Result<ItemGetResponse>.Fail("Not Found");
                }

                mapper.Map(command, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync();

                var result = await dbContext.Items
                    .Where(x => x.Id == command.Id)
                    .ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                var client = _httpFactory.CreateClient("NotificationService");
                await client.PostAsJsonAsync("api/notify", new WarehouseEvent
                {
                    EntityType = "Item",
                    AccountEmail = _user.Email,
                    Action = "Updated",
                    Description = $"Item {command.Name} was updated",
                    OccurredAt = DateTimeOffset.UtcNow
                }, ct);

                return Result<ItemGetResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<ItemGetResponse>.Fail(ex.Message);
            }
            */
            return null;
        }
    }
}