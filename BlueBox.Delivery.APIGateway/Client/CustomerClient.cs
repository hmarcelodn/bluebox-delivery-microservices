using BlueBox.Delivery.APIGateway.Model;
using MicroservicesNET.Platform;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlueBox.Delivery.APIGateway.Client
{
    public class CustomerClient : ICustomerClient
    {
        private static string customersBaseUrl = @"http://localhost:5001";
        private static string getCustomerListPathTemplate = @"/customers";
        private Policy _circuitBreakerPolicy =
                Policy.Handle<Exception>()
                      .CircuitBreakerAsync(5, TimeSpan.FromMinutes(2));

        private readonly IHttpClientFactory httpClientFactory;

        public CustomerClient()
        { }

        public CustomerClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            return await GetCustomerFromCustomersService();
        }

        protected async Task<IEnumerable<CustomerModel>> GetCustomerFromCustomersService()
        {
            var response = await _circuitBreakerPolicy.ExecuteAsync(() => RequestCustomerServiceFromCustomerService());

            return await ConvertToOrderCustomerList(response).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<CustomerModel>> ConvertToOrderCustomerList(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var customer = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(
                await response.Content.ReadAsStringAsync().ConfigureAwait(false)
            );

            return customer;
        }

        protected async Task<HttpResponseMessage> RequestCustomerServiceFromCustomerService()
        {
            using (var httpClient = await httpClientFactory.Create(new Uri(customersBaseUrl), "api_customers"))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await httpClient.GetAsync(getCustomerListPathTemplate)
                                       .ConfigureAwait(false);
            }
        }
    }
}
