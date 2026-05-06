
using Warehouse.Data;

namespace Warehouse.Services
{
    public interface IGenericService<GetDTO,SendDTO,UpdateDTO>
    {
        Task<Result<GetDTO>> findById(int id);
        Task<Result<IEnumerable<GetDTO>>> getAll();
        Task<Result<GetDTO>> edit(int id, UpdateDTO item);
        Task<Result<GetDTO>> add(SendDTO item);
        Task<Result<bool>> delete(int id);
    }
}
