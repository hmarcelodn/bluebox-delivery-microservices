namespace BlueBox.Delivery.APIGateway.Model
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
