using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RookieEShopper.Application.Service.Account;
using RookieEShopper.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

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
            services.AddScoped<IUserServices, UserServices>();
            
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
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async cxt =>
                        {
                            var userService = cxt.HttpContext.RequestServices.GetRequiredService<IUserServices>();

                            if (await userService.IsCustomerExist(new Guid(cxt.Principal.FindFirst("sub").Value)))
                                return;

                            var userInfoRequest = new UserInfoRequest
                            {
                                Address = options.Authority + "/connect/userinfo",
                                Token = cxt.SecurityToken.UnsafeToString()
                            };

                            using (var client = new HttpClient())
                            {
                                var response = await client.GetUserInfoAsync(userInfoRequest);

                                if (!response.Claims.IsNullOrEmpty())
                                {                                    
                                    var userId = new Guid(response.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
                                    var userName = response.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                                    var userEmail = response.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                                    var role = response.Claims.FirstOrDefault(x => x.Type == "role")?.Value;

                                    if (userId != Guid.Empty && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userEmail))
                                    {                                        
                                        await userService.EnsureUserExistsAsync(userId, userName, userEmail, role.IsNullOrEmpty()? "Customer" : role);
                                    }
                                }
                            }
                        }
                    };
                });            
        }
    }
}