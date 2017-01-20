using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Identity.Microservice
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResource() =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
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
