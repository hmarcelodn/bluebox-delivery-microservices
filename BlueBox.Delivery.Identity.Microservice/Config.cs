using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace BlueBox.Delivery.Identity.Microservice
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResource() =>
            new List<ApiResource>
            {
                new ApiResource("api_gateway", "API Gateway"),
                new ApiResource("api_customers", "API Customers")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "web.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api_gateway" }
                },
                new Client
                {
                    ClientId = "api.gateway.client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api_customers" }
                }                                
            };

        public static List<TestUser> GetUsers() =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
    }
}
