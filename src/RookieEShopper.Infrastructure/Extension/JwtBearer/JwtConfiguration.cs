using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RookieEShopper.Infrastructure.Extension.JwtBearer
{
    public static class JwtConfiguration
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.Bind(nameof(jwtOptions), jwtOptions);
            services.AddSingleton(jwtOptions);

            //services.AddAuthentication("Bearer")
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = "https://localhost:8899";

            //    options.Audience = "api.rookie";
            //    options.SaveToken = true;

            //    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            //});
        }

        public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Bearer";
            })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:8899";

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudiences = new List<string>
                            {"rookie.admin","rookie.customer"},
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GodScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", new List<string>{
                        "manage"
                    });
                });
                options.AddPolicy("CustomerScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", new List<string>{
                        "customer.read"
                    });
                });
            });
        }
    }
}