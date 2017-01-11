namespace BlueBox.Delivery.Customers.Microservice.DataModel
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
