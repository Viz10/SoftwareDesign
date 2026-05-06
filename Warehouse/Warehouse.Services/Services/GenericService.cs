using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public abstract class GenericService<DataType,GetDTO,SendDTO, UpdateDTO> : IGenericService<GetDTO, SendDTO, UpdateDTO>
    where DataType : class, ISoftDeletable /// must have Id , implement soft deletable prop and be ref type for accessing DB table
    where GetDTO : class
    where SendDTO : class
    where UpdateDTO : class
    {
        protected readonly WarehouseDbContext dbContext;
        protected readonly IMapper mapper;
       
        public GenericService(WarehouseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper= mapper;
        }

        public virtual async Task<Result<GetDTO>> findById(int id) 
        {
            try
            {
                var item = await dbContext.Set<DataType>().Where(el=>el.Id==id).ProjectTo<GetDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
                if (item == null) return Result<GetDTO>.Fail("Not Found");
                return Result<GetDTO>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<GetDTO>.Fail(ex.Message);
            }   
        }
        public virtual async Task<Result<IEnumerable<GetDTO>>> getAll()
        {
            try
            {
                var elements = await dbContext.Set<DataType>().ProjectTo<GetDTO>(mapper.ConfigurationProvider).ToListAsync();
                if(elements == null) return Result<IEnumerable<GetDTO>>.Fail("Not Found");
                return Result<IEnumerable<GetDTO>>.Success(elements);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<GetDTO>>.Fail(ex.Message);
            }
        }
        public virtual async Task<Result<GetDTO>> add(SendDTO item)
        {
            try /// children class must ensure not duplicate
            {
                var toBeAdded = mapper.Map<DataType>(item);
                await dbContext.Set<DataType>().AddAsync(toBeAdded);
                await dbContext.SaveChangesAsync();
                return Result<GetDTO>.Success(null);
            }
            catch (DbUpdateException ex)
            {
                return Result<GetDTO>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<GetDTO>.Fail(ex.Message);
            }
        }
        public virtual async Task<Result<GetDTO>> edit(int id, UpdateDTO updated)
        {
            try
            {
                var Old = await dbContext.Set<DataType>().FirstOrDefaultAsync(x => x.Id == id);

                if (Old == null)
                {
                    return Result<GetDTO>.Fail("Not Found");
                }

                mapper.Map(updated, Old);
                Old.LastModifiedTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync();

                var result = await dbContext.Set<DataType>()
                    .Where(x => x.Id == id)
                    .ProjectTo<GetDTO>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return Result<GetDTO>.Success(result);
            }
            catch (DbUpdateException ex)
            {
                return Result<GetDTO>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<GetDTO>.Fail(ex.Message);
            }
        }
        public virtual async Task<Result<bool>> delete(int id)
        {
            try
            {
             var item = await dbContext.Set<DataType>().FirstOrDefaultAsync(x => x.Id == id);
                
                if (item == null)
                {
                    return Result<bool>.Fail("Not Present");
                }

                item.IsDeleted = true;
                item.LastModifiedTime = DateTimeOffset.UtcNow;

                await dbContext.SaveChangesAsync();
                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
    }
}
