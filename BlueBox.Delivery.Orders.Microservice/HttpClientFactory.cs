using System;
using System.Net.Http;

namespace BlueBox.Delivery.Orders.Microservice
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string correlationToken;

        public HttpClientFactory(string correlationToken)
        {
            this.correlationToken = correlationToken;
        }

        public HttpClient Create(Uri uri)
        {
            var client = new HttpClient() { BaseAddress = uri };
            client.DefaultRequestHeaders.Add("Correlation-Token", this.correlationToken);

            return client;
        }
    }
}
