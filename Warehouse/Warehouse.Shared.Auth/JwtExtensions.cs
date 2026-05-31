using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Warehouse.Shared.Auth
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddSharedJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {

            // 1. Configures the JWT validation rules , parse and fill in claims
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["CreateJWT:Issuer"],
                        ValidAudience = configuration["CreateJWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["CreateJWT:Token"]!))
                    };
                });

            // 2. Registers the authorization engine
            services.AddAuthorization();
            return services;
        }
    }
}
