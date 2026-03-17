using Warehouse.Data.DTOs;

namespace Warehouse.Services
{
    public interface IServiceItem
    {
        Task<(ItemDTO?, bool)> findById(int id);
        Task<List<ItemResponseDTO>> getAllItems();
        Task addItem(ItemDTO item);
        Task<ItemResponseDTO?> editItem(int id, ItemDTO item);
        Task<bool> deleteItem(int id);
    }
}
