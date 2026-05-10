using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Warehouse.Client.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider // reads token, parses claims
    {
        private readonly ILocalStorageService _localStorage;

        public JwtAuthStateProvider(ILocalStorageService localStorage)
            => _localStorage = localStorage;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("token");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = ParseClaimsFromJwt(token); // reads claims from token directly
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task NotifyLogin(string token)
        {
            await _localStorage.SetItemAsStringAsync("token", token);
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task NotifyLogout()
        {
            await _localStorage.RemoveItemAsync("token");
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims;
        }
    }
}
