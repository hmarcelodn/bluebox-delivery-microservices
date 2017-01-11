using BlueBox.Delivery.Orders.Microservice.Client.DTO;
using BlueBox.Delivery.Orders.Microservice.DataModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Orders.Microservice.Client
{
    public class CustomersClient : ICustomersClient
    {
        private static string customersBaseUrl = @"http://localhost:5001";
        private static string getCustomerPathTemplate = @"/customers/{0}";

        public async Task<OrderCustomer> GetCustomer(int customerId)
        {
            return await GetCustomerFromCustomerService(customerId).ConfigureAwait(false);
        }

        protected async Task<OrderCustomer> GetCustomerFromCustomerService(int customerId)
        {
            var response = await RequestCustomerServiceFromCustomerService(customerId).ConfigureAwait(false);

            return await ConvertToOrderCustomer(response).ConfigureAwait(false);
        }

        protected async Task<OrderCustomer> ConvertToOrderCustomer(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var customerDto = JsonConvert.DeserializeObject<CustomerDTO>(
                await response.Content.ReadAsStringAsync().ConfigureAwait(false)
            );

            return new OrderCustomer()
            {
                CustomerId = customerDto .CustomerId,
                CustomerAddress = customerDto.Address
            };
        }

        protected async Task<HttpResponseMessage> RequestCustomerServiceFromCustomerService(int customerId)
        {
            var customersResource = string.Format(getCustomerPathTemplate, customerId);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(customersBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await httpClient.GetAsync(customersResource)
                                       .ConfigureAwait(false);
            }
        }
    }
}
