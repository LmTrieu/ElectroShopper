// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using RookieEcommerce.Auth.Services;
using System.Collections.Generic;

namespace RookieEcommerce.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                    new List<IdentityResource>
                    {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                    };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api.rookie", "RookieEcommerce API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new() {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    RedirectUris = { ReadConfig.clientUrls["mvc"] +"/signin-oidc" },
                    FrontChannelLogoutUri = ReadConfig.clientUrls["mvc"] +"/signout-callback-oidc",
                    PostLogoutRedirectUris = { ReadConfig.clientUrls["mvc"] +"/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.rookie"
                    }                
                },
                new() {
                    ClientId = "spa",
                    ClientName = "React app",
                    ClientUri = ReadConfig.clientUrls["react"],

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        ReadConfig.clientUrls["react"] +"/signin-oidc",
                    },
                    FrontChannelLogoutUri = ReadConfig.clientUrls["react"]+ "/signout-oidc",
                    PostLogoutRedirectUris = { ReadConfig.clientUrls["react"]+"/signout-callback-oidc" },
                    AllowedCorsOrigins = { ReadConfig.clientUrls["react"] },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.rookie"
                    }
                }
            };
    }
}