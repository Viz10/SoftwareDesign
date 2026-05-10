using AutoMapper;
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.AccountDTO;

namespace InventoryService.Application.Commands
{
    public record AddItemCommand(string Name, decimal? ReferencePricePerItem, string? Description) : IRequest<Result<string>>;

    internal class AddItemCommandHandler : IRequestHandler<AddItemCommand, Result<string>>
    {
        private readonly InventoryServiceDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ItemDomainService _itemDomainService;

        public AddItemCommandHandler(InventoryServiceDbContext _dbContext, IMapper _mapper, ItemDomainService itemDomainService)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            _itemDomainService = itemDomainService;
        }

        public async Task<Result<string>> Handle(AddItemCommand request, CancellationToken ct)
        {
            try
            {
                var result_dup = await _itemDomainService.isDuplicate(request.Name, null);
                if (!result_dup.IsSuccessful) return Result<string>.Fail(result_dup.ErrorMsg);
                if (result_dup.Value) return Result<string>.Fail("Duplicate Item");

                var result_restored = await _itemDomainService.Restore(request);
                if (!result_restored.IsSuccessful) return Result<string>.Fail(result_restored.ErrorMsg);

                if (result_restored.Value)
                {
                    /// restored
                    await dbContext.SaveChangesAsync(ct);
                }
                else
                {
                    /// add new
                    var toBeAdded = mapper.Map<Item>(request);
                    await dbContext.Items.AddAsync(toBeAdded);
                    await dbContext.SaveChangesAsync(ct);
                }
                return Result<string>.Success("Successfully added item!");
            }
            catch (DbUpdateException ex)
            {
                return Result<string>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }
    }
}
