using System;
using System.Net.Http;

namespace BlueBox.Delivery.Orders.Microservice
{
    public interface IHttpClientFactory
    {
        HttpClient Create(Uri uri);
    }
}
