using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Warehouse.Client.Auth
{
    public class AuthTokenHandler(ILocalStorageService localStorage) : DelegatingHandler /// for each request from blazor to server , attach jwt
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var token = await _localStorage.GetItemAsStringAsync("token", ct);
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, ct);
        }
    }
}