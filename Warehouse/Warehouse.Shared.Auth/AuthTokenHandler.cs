
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Warehouse.Shared.Auth
{
    public class AuthTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler /// for each request from blazor to server , attach jwt
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (AuthenticationHeaderValue.TryParse(authHeader, out var headerValue))
                {
                    request.Headers.Authorization = headerValue;
                }
            }
            return await base.SendAsync(request, ct);
        }
    }
}