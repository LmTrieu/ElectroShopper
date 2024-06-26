﻿using Microsoft.OpenApi.Models;
using RookieEShopper.Api.Middlewares;

namespace RookieEShopper.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GodScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", new List<string>{
                        "manage"
                    });
                    policy.RequireClaim("role", new List<string>{
                        "Admin"
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

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RookieEcommerce API", Version = "v1" });

                options.AddSecurityDefinition("oidc", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri("https://localhost:8899/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:8899/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:8899/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"api.rookie", "RookieEcommerce API"}
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oidc" }
                        },
                        new List<string>{ "api.rookie" }
                    }
                });
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }
    }
}