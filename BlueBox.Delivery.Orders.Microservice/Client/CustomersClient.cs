using BlueBox.Delivery.Orders.Domain.Domain;
using BlueBox.Delivery.Orders.Microservice.Client.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using System.Net.Http.Headers;
using MicroservicesNET.Platform;

namespace BlueBox.Delivery.Orders.Microservice.Client
{
    public class CustomersClient : ICustomersClient
    {
        private static string customersBaseUrl = @"http://localhost:5001";
        private static string getCustomerPathTemplate = @"/customers/{0}";
        private Policy _circuitBreakerPolicy = 
            Policy.Handle<Exception>()
                  .CircuitBreakerAsync(5, TimeSpan.FromMinutes(2));

        private readonly IHttpClientFactory httpClientFactory;

        public CustomersClient()
        { }

        public CustomersClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            return await GetCustomerFromCustomerService(customerId).ConfigureAwait(false);
        }

        protected async Task<Customer> GetCustomerFromCustomerService(int customerId)
        {
            var response = await _circuitBreakerPolicy.ExecuteAsync(
                () => RequestCustomerServiceFromCustomerService(customerId));

            return await ConvertToOrderCustomer(response).ConfigureAwait(false);
        }

        protected async Task<Customer> ConvertToOrderCustomer(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var customerDto = JsonConvert.DeserializeObject<CustomerDTO>(
                await response.Content.ReadAsStringAsync().ConfigureAwait(false)
            );

            return new Customer()
            {
                CustomerId = customerDto .CustomerId,
                CustomerAddress = customerDto.Address
            };
        }

        protected async Task<HttpResponseMessage> RequestCustomerServiceFromCustomerService(int customerId)
        {
            var customersResource = string.Format(getCustomerPathTemplate, customerId);

            var httpClient = await httpClientFactory.Create(new Uri(customersBaseUrl), "api_customers");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await httpClient.GetAsync(customersResource)
                                   .ConfigureAwait(false);
        }
    }
}
