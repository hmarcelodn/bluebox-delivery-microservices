using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroservicesNET.Platform
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string correlationToken;
        private readonly TokenClient tokenClient;

        public HttpClientFactory(string correlationToken)
        {
            this.correlationToken = correlationToken;
            this.tokenClient = new TokenClient("http://localhost:5003/connect/token", "api.gateway.client", "secret");
        }

        public async Task<HttpClient> Create(Uri uri, string scope)
        {
            var response = await this.tokenClient
                                     .RequestClientCredentialsAsync(scope)
                                     .ConfigureAwait(false);

            var client = new HttpClient() { BaseAddress = uri };

            client.DefaultRequestHeaders
                  .Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", response.AccessToken);

            client
                .DefaultRequestHeaders.Add("Correlation-Token", this.correlationToken);

            return client;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
