using BlueBox.Delivery.Orders.Domain.Domain;
using System;
using System.Collections.Generic;

namespace BlueBox.Delivery.Orders.Domain.Storage
{
    public interface IOrderStorage
    {
        Order GetOrderByOrderId(Guid orderId);

        IEnumerable<Order> GetOrdersByCustomerId(int customerId);

        void Save(Order o);
    }
}
