using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Warehouse.Shared.Auth
{
    public class CurrentUser(IHttpContextAccessor _accessor)
    {
        private readonly IHttpContextAccessor accessor = _accessor;

        public int Id => int.Parse(accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public string Email => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
        public string Role => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;
    }
}
