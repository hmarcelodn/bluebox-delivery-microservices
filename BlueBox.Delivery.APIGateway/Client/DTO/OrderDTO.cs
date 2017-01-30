using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueBox.Delivery.APIGateway.Client.DTO
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; protected set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string CustomerAddress { get; set; }

        public string DeliveryAddressTo { get; set; }
    }
}
