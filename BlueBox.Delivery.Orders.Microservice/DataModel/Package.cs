using System;

namespace BlueBox.Delivery.Orders.Microservice.DataModel
{
    public class Package
    {
        public Package(double weight, double width, double height, string deliveryAddress)
        {
            DeliveryAddressTo = deliveryAddress;
            PackageWeight = weight;
            PackageHeight = height;
            PackageWidth = width;
        }

        public DateTime DeliveryDate { get; protected set; }

        public DateTime ReceivedDate { get; protected set; }

        public string DeliveryAddressTo { get; protected set; }

        public string PackageDescription { get; protected set; }

        public double PackageWeight { get; protected set; }

        public double PackageWidth { get; protected set; }

        public double PackageHeight { get; protected set; }

        public void SetDeliveryDate(DateTime deliveryDate)
        {
            DeliveryDate = deliveryDate;
        }

        public void MarkAsDelivered()
        {
            ReceivedDate = DateTime.UtcNow;
        }
    }
}
