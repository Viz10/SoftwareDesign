
namespace Warehouse.Services
{
    public interface IGenericService<GetDTO,SendDTO>
    {

        Task<(GetDTO? Value, string? Error)> findById(int id);
        Task<(IEnumerable<GetDTO>? Value, string? Error)> getAll();
        Task<(GetDTO? Value,string? Error)> edit(int id, SendDTO item);
        Task<string?> add(SendDTO item);
        Task<string?> delete(int id);
    }
}
