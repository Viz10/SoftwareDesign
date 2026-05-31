using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Queries
{
    internal record GetEveryItemQuery(string? Name, string? SortBy) : IRequest<Result<List<ItemGetResponse>>>;


    internal class GetEveryItemQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<GetEveryItemQuery, Result<List<ItemGetResponse>>>
    {

        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result<List<ItemGetResponse>>> Handle(GetEveryItemQuery get_query, CancellationToken cancellationToken)
        {
            try
            {
                var dbQuery = dbContext.Items.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(get_query.Name) && get_query.Name.Length >= 2)
                {
                    dbQuery = dbQuery.Where(item => item.Name.Contains(get_query.Name, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(get_query.SortBy))
                {
                    dbQuery = get_query.SortBy.Equals("descending", StringComparison.OrdinalIgnoreCase)
                        ? dbQuery.OrderByDescending(el => el.Name)
                        : dbQuery.OrderBy(el => el.Name);
                }

                var result = await dbQuery
                    .ProjectTo<ItemGetResponse>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<ItemGetResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<ItemGetResponse>>.Fail(ex.Message);
            }
        }
    }
}
