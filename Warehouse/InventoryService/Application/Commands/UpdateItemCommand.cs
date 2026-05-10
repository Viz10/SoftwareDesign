using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Globalization;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryService.Application.Commands
{
    public record UpdateItemCommand(int id,string Name, decimal? ReferencePricePerItem, string? Description) : IRequest<Result<ItemGetResponse>>;

    internal class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<ItemGetResponse>>
    {
        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ItemDomainService _itemDomainService;

        public UpdateItemCommandHandler(InventoryServiceDbContext _dbContext, IMapper _mapper, ItemDomainService itemDomainService)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            _itemDomainService = itemDomainService;
        }

        public async Task<Result<ItemGetResponse>> Handle(UpdateItemCommand command, CancellationToken ct)
        {
            try
            {
                var result_dup = await _itemDomainService.isDuplicate(command.Name, command.id);
                if (!result_dup.IsSuccessful) { return Result<ItemGetResponse>.Fail(result_dup.ErrorMsg); }
                if (result_dup.Value) { return Result<ItemGetResponse>.Fail("Duplicate Item"); }

                var Old = await dbContext.Items.FindAsync(command.id);

                if (Old == null)
                {
                    return Result<ItemGetResponse>.Fail("Not Found");
                }

                mapper.Map(command, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync();

                var result = await dbContext.Items
                    .Where(x => x.Id == command.id)
                    .ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return Result<ItemGetResponse>.Success(result);

                /*
                 _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Edited",
                    Description = $"Edited to: {updated.Name}\n{updated.ReferencePricePerItem}\n{updated.Description}",
                });
                 */
            }
            catch (Exception ex)
            {
                return Result<ItemGetResponse>.Fail(ex.Message);
            }


        }
    }
}
