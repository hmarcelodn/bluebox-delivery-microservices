using System;

namespace BlueBox.Delivery.Orders.Microservice.Model
{
    public class PackageModel
    {
        public Guid OrderId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string DeliveryAddressTo { get; set; }

        public string PackageDescription { get; set; }

        public double PackageWeight { get; set; }

        public double PackageWidth { get; set; }

        public double PackageHeight { get; set; }
    }
}
