using System.Linq;
using IdentityServer4.Models;

namespace Grw.Gin.Auth.Models
{
    public class ClientAuthConfig : Client
    {
        private string[] clientSecretsNormal;
        public string[] ClientSecretsNormal
        {
            get
            {
                return clientSecretsNormal;
            }
            set
            {
                clientSecretsNormal = value;
                ClientSecrets = clientSecretsNormal.Select(v => new Secret(v.Sha256())).ToArray();
            }
        }

        public string AllowedGrantTypesName { get; set; }
    }
}
