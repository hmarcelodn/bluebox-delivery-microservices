using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using BlueBox.Delivery.APIGateway.Client.DTO;
using BlueBox.Delivery.APIGateway.Model;
using System.Text;

namespace BlueBox.Delivery.APIGateway.Client
{
    public class OrderClient : IOrderClient
    {
        private static string ordersBaseUrl = @"http://localhost:5002";
        private static string newOrderPathTemplate = @"/orders";
        private static string newPackagePathTemplate = @"/orders/package";
        private static string customerOrdersPathTemplate = @"/orders/{0}";

        public async Task<Guid> CreateOrderFromOrdersService(int customerId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ordersBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestParams = new Dictionary<string, string>()
                {
                    { "CustomerId", customerId.ToString() }
                };

                var content = new FormUrlEncodedContent(requestParams);

                var httpResponse = await httpClient.PostAsync(newOrderPathTemplate, content).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();

                var orderData = JsonConvert.DeserializeObject<NewOrderDTO>(
                        await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );

                return orderData.OrderId;
            }
        }

        public async Task UpdateOrderWithPackageFromOrdersService(Guid orderId, NewOrderModel orderModel)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ordersBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestParams = new Dictionary<string, string>()
                {
                    { "OrderId", orderId.ToString() },
                    { "DeliveryDate", orderModel.DeliveryDate.ToString() },
                    { "DeliveryAddressTo", orderModel.DeliveryAddressTo.ToString() },
                    { "PackageDescription", orderModel.PackageDescription },
                    { "PackageWeight", orderModel.PackageWeight.ToString() },
                    { "PackageWidth", orderModel.PackageWidth.ToString() },
                    { "PackageHeight", orderModel.PackageHeight.ToString() }
                };

                var content = new FormUrlEncodedContent(requestParams);

                var httpResponse = await httpClient.PutAsync(newPackagePathTemplate, content).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();
            }
        }

        public async Task<IEnumerable<object>> GetAllCustomerOrdersFromOrdersService(int customerId)
        {
            var customerOrdersResource = string.Format(customerOrdersPathTemplate, customerId);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(customerOrdersPathTemplate);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(new { customerId = customerId }));
                var httpResponse = await httpClient.GetAsync(customerOrdersResource).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();
                var orderData = JsonConvert.DeserializeObject<IEnumerable<object>>(
                        await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );

                return orderData;
            }
        }

        public Task<IEnumerable<object>> GetAllOrdersFromOrdersService()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetOrderFromOrdersService(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
