using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Orders.Domain.Domain
{
    public class Order
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
        }

        public Guid OrderId { get; protected set; }

        public int CustomerId { get; protected set; }

        public DateTime OrderDate { get; protected set; }

        public DateTime DeliveryDate { get; protected set; }

        public DateTime ReceivedDate { get; protected set; }

        public string CustomerAddress { get; protected set; }

        public string DeliveryAddressTo { get; protected set; }

        public OrderStatus OrderStatus { get; protected set; }

        public Package PackageToDeliver { get; protected set; }

        public void ChangeOrderStatus(OrderStatus orderStatus)
        {
            OrderStatus = orderStatus;
        }

        public void SetPackageInformation(Package package, DateTime deliveryDate)
        {
            PackageToDeliver = package;
            PackageToDeliver.SetDeliveryDate(deliveryDate);
        }

        public void SetCustomerInformation(Customer orderCustomer)
        {
            CustomerId = orderCustomer.CustomerId;
            CustomerAddress = orderCustomer.CustomerAddress;
        }

        public static Order Instance()
        {
            return new Order();
        }
    }
}
