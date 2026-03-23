
namespace Warehouse.Services
{
    public interface IGenericService<ResponseDTO,CreateDTO,UpdateDTO>
    {
        Task<ResponseDTO?> findById(int id);
        Task<IEnumerable<ResponseDTO>> getAll();
        Task<CreateDTO?> add(CreateDTO item);
        Task<UpdateDTO?> edit(int id, UpdateDTO item);
        Task<bool> delete(int id);
    }
}
