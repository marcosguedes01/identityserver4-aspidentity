using System.Collections.Generic;
using Grw.Gin.Auth.Exceptions;
using IdentityServer4.Models;

namespace Grw.Gin.Auth.Configurations
{
    public class AuthVariables
    {
        public static IdentityResource GetIdentityResource(string name)
        {
            switch (name.ToLower())
            {
                case "openid":
                    return new IdentityResources.OpenId();
                case "profile":
                    return new IdentityResources.Profile();
                case "email":
                    return new IdentityResources.Email();
                case "phone":
                    return new IdentityResources.Phone();
                case "address":
                    return new IdentityResources.Address();
            }

            throw new GrwGinAuthException("Identity Resource is not found.");
        }

        public static ICollection<string> GetAllowedGrantTypes(string name)
        {
            switch (name.ToLower())
            {
                case "implicit":
                    return GrantTypes.Implicit;
                case "implicitandclientcredentials":
                    return GrantTypes.ImplicitAndClientCredentials;
                case "Code":
                    return GrantTypes.Code;
                case "codeandclientcredentials":
                    return GrantTypes.CodeAndClientCredentials;
                case "hybrid":
                    return GrantTypes.Hybrid;
                case "hybridandclientcredentials":
                    return GrantTypes.HybridAndClientCredentials;
                case "clientcredentials":
                    return GrantTypes.ClientCredentials;
                case "resourceownerpassword":
                    return GrantTypes.ResourceOwnerPassword;
                case "resourceownerpasswordandclientcredentials":
                    return GrantTypes.ResourceOwnerPasswordAndClientCredentials;
                case "deviceflow":
                    return GrantTypes.DeviceFlow;
            }

            throw new GrwGinAuthException("Allowed Grant Types is not found.");
        }
    }
}
