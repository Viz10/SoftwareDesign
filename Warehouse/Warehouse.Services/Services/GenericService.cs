using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public abstract class GenericService<DataType,GetDTO,SendDTO> : IGenericService<GetDTO, SendDTO>
    where DataType : class, IEntity /// like c++ requires(T t){t.Id;} costraint
    where GetDTO : class
    where SendDTO : class /// to be able to use null on them
    {
        protected readonly WarehouseDbContext dbContext;
        protected readonly IMapper mapper;
       
        public GenericService(WarehouseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper= mapper;
        }

        public virtual async Task<(GetDTO? Value, string? Error)> findById(int id) 
        {
            try
            {
                var item = await dbContext.Set<DataType>().FindAsync(id);
                if (item == null) return (null,"Not Found");
                return (mapper.Map<GetDTO>(item),null);
            }
            catch (Exception ex)
            {
                return (null,ex.Message);
            }   
        }
        public virtual async Task<(IEnumerable<GetDTO>? Value, string? Error)> getAll()
        {
            try
            {
                var elements = await dbContext.Set<DataType>().ToListAsync();
                if(elements == null) return (null,"Not found");
                return (mapper.Map<IEnumerable<GetDTO>>(elements), null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public virtual async Task<string?> add(SendDTO item)
        {
            try
            {
                var toBeAdded = mapper.Map<DataType>(item);
                await dbContext.Set<DataType>().AddAsync(toBeAdded); /// as its generic cannot check weather is already there by name or other props.
                await dbContext.SaveChangesAsync();
                return null;
            }
            catch (DbUpdateException)
            {
                return "Already present!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public virtual async Task<(GetDTO? Value, string? Error)> edit(int id, SendDTO updated)
        {
            try
            {
                var Old = await dbContext.Set<DataType>().FirstOrDefaultAsync(x => x.Id == id);

                if (Old == null)
                {
                    return (null,"Not found"); /// not found
                }
                mapper.Map(updated, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync();
                return (mapper.Map<GetDTO>(Old), null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public virtual async Task<string?> delete(int id)
        {
            try
            {
             var item = await dbContext.Set<DataType>().FirstOrDefaultAsync(x => x.Id == id);
                
                if (item == null)
                {
                    return "Not present!";
                }

                item.IsDeleted = true;
                item.DeletedAtTime = DateTimeOffset.UtcNow;
                item.LastModifiedTime = DateTimeOffset.UtcNow;

                await dbContext.SaveChangesAsync();
                return null;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public virtual async Task<string?> restore(int id)
        {
            var item = await dbContext.Set<DataType>().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);

            if (item == null) return "Not found";

            item.IsDeleted = false;
            item.DeletedAtTime = null;
            item.LastModifiedTime = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync();
            return null;
        }
    }
}
