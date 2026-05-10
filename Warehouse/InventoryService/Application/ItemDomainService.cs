using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Infrastructure.DbRepository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;

namespace InventoryService.Application
{
    public class ItemDomainService(InventoryServiceDbContext dbContext,IMapper mapper)
    {
        /// Check items for duplicate when adding/editing
        public async Task<Result<bool>> isDuplicate(string name, int? editItemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<bool>.Success(false);
                }

                bool exists;
                if (editItemId.HasValue)
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower() && item.Id != editItemId);
                }
                else
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower());
                }

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
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

                    mapper.Map(itemSend, deletedItem);

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
