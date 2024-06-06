// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using RookieEcommerce.Auth.Services;

namespace RookieEcommerce.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                    new List<IdentityResource>
                    {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(
                    name: "roles",
                    userClaims: [JwtClaimTypes.Role]),
                new IdentityResource(
                    name: "customer.claims",
                    userClaims: ["customer.id"]),
                    };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("rookie.admin")
                {
                    Scopes = {
                        "manage"
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    }
                },
                new ApiResource("rookie.customer", "RookieEcommerce API for customer")
                {
                    Scopes = {
                        "customer.read",
                        "customer.write"
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Role
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(name: "manage",   displayName: "Manage your data."),

                new ApiScope(name: "customer.read",    displayName: "Reads you customers information."),
                new ApiScope(name: "customer.write", displayName: "Allows customer inputs."),
            };
        }

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new() {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RequireClientSecret = true,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        ReadConfig.clientUrls["mvc"] +"/signin-oidc"
                    },
                    FrontChannelLogoutUri = ReadConfig.clientUrls["mvc"],
                    PostLogoutRedirectUris = { ReadConfig.clientUrls["mvc"] + "/signout-callback-oidc" },

                    AllowedCorsOrigins = {ReadConfig.clientUrls["mvc"] },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "rookie.customer",
                        "customer.read",
                        "customer.write",
                        "customer.claims",
                    }
                },
                new() {
                    ClientId = "spa",
                    ClientName = "React app",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    ClientUri = ReadConfig.clientUrls["react"],

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = true,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        ReadConfig.clientUrls["react"],
                    },
                    FrontChannelLogoutUri = ReadConfig.clientUrls["react"],
                    PostLogoutRedirectUris = { ReadConfig.clientUrls["react"]},
                    AllowedCorsOrigins = { ReadConfig.clientUrls["react"] },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        "account.role",
                        "manage"
                    }
                },
                new() {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = { ReadConfig.clientUrls["swagger"] +"/swagger/oauth2-redirect.html" },
                    FrontChannelLogoutUri = ReadConfig.clientUrls["swagger"] +"/signout-callback-oidc",
                    PostLogoutRedirectUris = { ReadConfig.clientUrls["swagger"] +"/signout-callback-oidc" },
                    AllowedCorsOrigins = { ReadConfig.clientUrls["swagger"] },
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "rookie.admin",
                        "roles",
                        "account.role",
                        "manage",
                        "customer.claims"
                    }
                }
            };
    }
}