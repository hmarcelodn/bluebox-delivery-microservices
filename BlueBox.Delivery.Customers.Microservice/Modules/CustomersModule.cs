using Nancy;
using BlueBox.Delivery.Customers.Microservice.Storage;

namespace BlueBox.Delivery.Customers.Microservice.Modules
{
    public class CustomersModule : NancyModule
    {
        public CustomersModule(ICustomerStorage customerStorage) 
            : base("/customers")
        {
            Get("/{customerid:int}", parameters =>
            {
                var customerId = (int)parameters.customerid;

                return customerStorage.GetCustomerById(customerId);
            });
        }
    }
}
