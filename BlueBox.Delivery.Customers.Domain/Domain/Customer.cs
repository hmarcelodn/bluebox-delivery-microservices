namespace BlueBox.Delivery.Customers.Domain.Domain
{
    public class Customer
    {
        public string Address { get; set; }

        public int CustomerId { get; set; }

        public string DeliveryAddress { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
