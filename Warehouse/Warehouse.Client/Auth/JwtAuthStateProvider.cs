using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Warehouse.Client.Auth
{
    public class JwtAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider 
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Email, ClaimTypes.Role);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        public async Task NotifyLogin(string token)
        {
            await _localStorage.SetItemAsStringAsync("token", token);
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Email, ClaimTypes.Role);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)))); /// notify UI for authorization
        }
        public async Task NotifyLogout()
        {
            await _localStorage.RemoveItemAsync("token");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims;
        }
    }
}
