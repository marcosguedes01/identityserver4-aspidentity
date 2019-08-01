#define IN_MEMORY

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Grw.Gin.Auth.Models;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace Grw.Gin.Auth.Configurations
{
    public static class AuthConfig
    {
        public static IEnumerable<ApiResource> GetApis(IConfiguration configuration)
        {
            var resourcesConfig = new List<ApiResource>();
            configuration.GetSection("ApplicationSettings").GetSection("AuthConfig").GetSection("ApiResources").Bind(resourcesConfig);

            return resourcesConfig.Select(r => new ApiResource(r.Name, r.DisplayName) { UserClaims = r.UserClaims });
        }

        public static IEnumerable<IdentityResource> GetIdentityResources(IConfiguration configuration)
        {
            var resourcesConfig = new List<string>();
            configuration.GetSection("ApplicationSettings").GetSection("AuthConfig").GetSection("IdentityResources").Bind(resourcesConfig);

            return resourcesConfig.Select(r => AuthVariables.GetIdentityResource(r)).ToArray();
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var resourcesConfig = new List<ClientAuthConfig>();
            configuration.GetSection("ApplicationSettings").GetSection("AuthConfig").GetSection("Clients").Bind(resourcesConfig);

            return resourcesConfig.Select(r => new Client
            {
                ClientId = r.ClientId,
                ClientName = r.ClientName,
                ClientSecrets = r.ClientSecrets,
                AllowedGrantTypes = AuthVariables.GetAllowedGrantTypes(r.AllowedGrantTypesName),
                AllowedScopes = r.AllowedScopes,
                AllowAccessTokensViaBrowser = r.AllowAccessTokensViaBrowser,
                RedirectUris = r.RedirectUris,
                PostLogoutRedirectUris = r.PostLogoutRedirectUris,
                AllowedCorsOrigins = r.AllowedCorsOrigins,
                RequireConsent = r.RequireConsent
            });
        }

#if IN_MEMORY
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId= "s4ksHuV4vX",
                    Username = "DESCONHECIDO@DESCONHECIDO.pt",
                    Password = "DESCONHECIDO",
                    Claims = new [] { new Claim("email", "DESCONHECIDO@DESCONHECIDO.pt") }
                }
            };
        }
#endif
    }
}
