using BlueBox.Delivery.APIGateway.Model;
using Newtonsoft.Json;
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

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            return await GetCustomerFromCustomersService();
        }

        protected async Task<IEnumerable<CustomerModel>> GetCustomerFromCustomersService()
        {
            var response = await RequestCustomerServiceFromCustomerService().ConfigureAwait(false);

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
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(customersBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await httpClient.GetAsync(getCustomerListPathTemplate)
                                       .ConfigureAwait(false);
            }
        }
    }
}
