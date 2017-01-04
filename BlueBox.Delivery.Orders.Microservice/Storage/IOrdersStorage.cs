using BlueBox.Delivery.Orders.Microservice.Aggregates;
using System;
using System.Collections.Generic;

namespace BlueBox.Delivery.Orders.Microservice.Storage
{
    public interface IOrdersStorage
    {
        Order GetOrderByOrderId(Guid orderId);

        IEnumerable<Order> GetOrdersByCustomerId(int customerId);

        void Save(Order o);
    }
}
