using Microsoft.EntityFrameworkCore;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class ServiceItem(WarehouseDbContext DbContext) : IServiceItem
    {
        private WarehouseDbContext _dbContext { get; } = DbContext;


        public async Task<(ItemDTO?,bool)> findById(int id)
        {
            var item = await _dbContext.Items.FindAsync(id);

            if(item == null) return (null,false);

            return (new ItemDTO
            {
                Description=item.Description,
                Name=item.Name,
                PricePerItem=item.PricePerItem
            },true);
        }
        public async Task addItem(ItemDTO item)
        {
            _dbContext.Items.Add(new Item{
                Name=item.Name,
                Description=item.Description,
                PricePerItem=item.PricePerItem ?? 0
            });
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> deleteItem(int id)
        {
            var item = await _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return false;
            }

            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<ItemResponseDTO?> editItem(int id,ItemDTO item)
        {
            var it = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (it == null)
            {
                return null;
            }

            it.PricePerItem = item.PricePerItem ?? 0;
            it.Description = item.Description ?? null;
            it.Name = item.Name;

            await _dbContext.SaveChangesAsync();

            return new ItemResponseDTO
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                PricePerItem = item.PricePerItem ?? 0
            };
        }
        public async Task<List<ItemResponseDTO>> getAllItems() => await _dbContext.Items.Select(el =>
            new ItemResponseDTO {
                Id= el.Id,
                Name = el.Name,
                Description = el.Description,
                PricePerItem = el.PricePerItem 
            }).ToListAsync();
    }
}
