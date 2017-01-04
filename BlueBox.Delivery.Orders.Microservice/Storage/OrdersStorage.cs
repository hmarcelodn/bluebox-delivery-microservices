using BlueBox.Delivery.Orders.Microservice.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueBox.Delivery.Orders.Microservice.Storage
{
    public class OrdersStorage : IOrdersStorage
    {
        private static IList<Order> _database = new List<Order>();

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            return _database.Where(c => c.CustomerId == customerId).AsEnumerable();
        }

        public Order GetOrderByOrderId(Guid orderId)
        {
            var order = TryGetOrderByOrderId(orderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            return order;
        }

        public void Save(Order o)
        {
            var order = TryGetOrderByOrderId(o.OrderId);

            if (order == null)
            {
                _database.Add(o);
            }
            else
            {
                _database.Remove(order);
                _database.Add(o);
            }
        }

        private Order TryGetOrderByOrderId(Guid orderId)
        {
            var order = _database.Where(o => o.OrderId == orderId).FirstOrDefault();

            return order;
        }
    }
}
