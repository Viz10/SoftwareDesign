using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Warehouse.Shared.Auth
{
    public class CurrentUser
    {
        private readonly IHttpContextAccessor accessor;
        public CurrentUser(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
        }

        public int Id => int.Parse(accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public string Email => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
        public string Role => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;
    }
}
