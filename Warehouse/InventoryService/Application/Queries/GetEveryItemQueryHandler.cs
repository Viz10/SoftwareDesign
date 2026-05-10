using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryService.Application.Commands;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Queries
{
    public record GetEveryItemQuery(string? name, string? sortBy) : IRequest<Result<List<ItemGetResponse>>>;

    public class GetEveryItemQueryHandler : IRequestHandler<GetEveryItemQuery, Result<List<ItemGetResponse>>>
    {

        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;

        public GetEveryItemQueryHandler(InventoryServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<List<ItemGetResponse>>> Handle(GetEveryItemQuery get_query, CancellationToken cancellationToken)
        {
            try
            {
                var dbQuery = dbContext.Items.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(get_query.name) && get_query.name.Length >= 2)
                {
                    dbQuery = dbQuery.Where(item => item.Name.ToLower().Contains(get_query.name.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(get_query.sortBy))
                {
                    dbQuery = get_query.sortBy.Equals("descending", StringComparison.OrdinalIgnoreCase)
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
