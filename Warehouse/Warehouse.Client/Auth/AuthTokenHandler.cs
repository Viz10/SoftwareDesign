using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Warehouse.Client.Auth
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthTokenHandler(ILocalStorageService localStorage)
            => _localStorage = localStorage;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken ct)
        {
            var token = await _localStorage.GetItemAsStringAsync("token");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, ct);
        }
    }
}