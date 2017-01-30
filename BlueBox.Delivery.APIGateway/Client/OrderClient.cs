using BlueBox.Delivery.APIGateway.Client.DTO;
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
    public class OrderClient : IOrderClient
    {
        private static string ordersBaseUrl = @"http://localhost:5002";
        private static string newOrderPathTemplate = @"/orders";
        private static string newPackagePathTemplate = @"/orders/package";
        private static string customerOrdersPathTemplate = @"/orders/{0}";

        private Policy _circuitBreakerPolicy =
            Policy.Handle<Exception>()
                  .CircuitBreakerAsync(5, TimeSpan.FromMinutes(2));

        private readonly IHttpClientFactory httpClientFactory;

        public OrderClient()
        { }

        public OrderClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // TODO: Add Circuit Breaker Policy
        public async Task<Guid> CreateOrderFromOrdersService(int customerId)
        {
            using (var httpClient = await this.httpClientFactory.Create(new Uri(ordersBaseUrl), "api_orders"))
            {
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

        // TODO: Add Circuit Breaker Policy
        public async Task UpdateOrderWithPackageFromOrdersService(Guid orderId, NewOrderModel orderModel)
        {
            using (var httpClient = await this.httpClientFactory.Create(new Uri(ordersBaseUrl), "api_orders"))
            {
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

        // TODO: Add Circuit Breaker Policy
        public async Task<IEnumerable<OrderDTO>> GetAllCustomerOrdersFromOrdersService(int customerId)
        {
            var customerOrdersResource = string.Format(customerOrdersPathTemplate, customerId);

            using (var httpClient =  await httpClientFactory.Create(new Uri(ordersBaseUrl), "api_orders"))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(new { customerId = customerId }));
                var httpResponse = await httpClient.GetAsync(customerOrdersResource).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();
                var orderData = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(
                        await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );

                return orderData;
            }
        }

        // TODO: Add Circuit Breaker Policy
        public async Task<IEnumerable<object>> GetAllOrdersFromOrdersService()
        {
            throw new NotImplementedException();
        }

        // TODO: Add Circuit Breaker Policy
        public async Task<object> GetOrderFromOrdersService(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
