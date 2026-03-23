using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public abstract class GenericService<DataType, ResponseDTO, CreateDTO, UpdateDTO> : IGenericService<ResponseDTO, CreateDTO, UpdateDTO>
    where DataType : class, IEntity /// like c++ requires(T t){t.Id;} costraint
    where ResponseDTO : class
    where CreateDTO : class
    where UpdateDTO : class /// to be able to use null on them
    {
        protected readonly DbContext dbContext;
        protected readonly IMapper mapper;
        public GenericService(WarehouseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper= mapper;
        }

        public virtual async Task<ResponseDTO?> findById(int id) 
        {
            var item = await dbContext.Set<DataType>().FindAsync(id);

            if (item == null) return null; /// not found

            return mapper.Map<ResponseDTO>(item);
        }
        public virtual async Task<IEnumerable<ResponseDTO>> getAll()
        {
            var elements = await dbContext.Set<DataType>().ToListAsync();

            return mapper.Map<IEnumerable<ResponseDTO>>(elements);
        }
        public virtual async Task<CreateDTO?> add(CreateDTO item)
        {
            var newType = mapper.Map<DataType>(item);
            try
            {
                await dbContext.Set<DataType>().AddAsync(newType);
                await dbContext.SaveChangesAsync();
                return item;
            }
            catch (DbException)
            {
                return null; /// already in
            }
        }
        public virtual async Task<UpdateDTO?> edit(int id, UpdateDTO item)
        {
            var it = await dbContext.Set<DataType>().FirstOrDefaultAsync(x => x.Id == id);

            if (it == null)
            {
                return null; /// not found
            }

            mapper.Map(item, it);

            await dbContext.SaveChangesAsync();

            return item;
        }
        public virtual async Task<bool> delete(int id)
        {
            var item = await dbContext.Set<DataType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return false;
            }

            dbContext.Set<DataType>().Remove(item);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
