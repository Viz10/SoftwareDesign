using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Infrastructure.DbRepository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;

namespace InventoryService.Application.DomainService
{
    public class ItemDomainService(InventoryServiceDbContext dbContext,IMapper mapper)
    {
        /// Check items for duplicate when adding/editing
        public async Task<Result> isDuplicate(string name, int? editItemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Success();
                }

                bool exists=false;
                
                if (editItemId.HasValue)
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower() && item.Id != editItemId);
                }
                else
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower());
                }

                return exists ? Result.Fail("Duplicate item") : Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        
        /// only mutate data , handle business logic 
        /// Instead of adding new, restore the old one with its stock with new data
        public async Task<Result<bool>> Restore(AddItemCommand itemSend)
        {
            try
            {
                var deletedItem = await dbContext.Items.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == itemSend.Name.ToLower() && i.IsDeleted);

                if (deletedItem != null)
                {
                    deletedItem.IsDeleted = false;
                    deletedItem.LastModifiedTime = DateTimeOffset.UtcNow;

                    mapper.Map(itemSend, deletedItem); /// provide with new values

                    var stock = await dbContext.Stocks.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.ItemId == deletedItem.Id);

                    if (stock != null)
                        stock.IsDeleted = false;
                    
                    return Result<bool>.Success(true); /// restored
                }
                return Result<bool>.Success(false); /// not restored
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
        
        /// Restore one from deleted list and also its stock
        public async Task<Result<bool>> Restore(int itemId)
        {
            try
            {
                var deletedItem = await dbContext.Items.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(i => i.Id == itemId && i.IsDeleted);

                if (deletedItem != null)
                {
                    deletedItem.IsDeleted = false;
                    deletedItem.LastModifiedTime = DateTimeOffset.UtcNow;

                    var stock = await dbContext.Stocks.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.ItemId == deletedItem.Id);

                    if (stock != null)
                        stock.IsDeleted = false;

                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
    }
}
