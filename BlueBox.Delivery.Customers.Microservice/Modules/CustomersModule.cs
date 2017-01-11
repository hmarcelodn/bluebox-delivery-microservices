using Nancy;
using BlueBox.Delivery.Customers.Microservice.Storage;

namespace BlueBox.Delivery.Customers.Microservice.Modules
{
    public class CustomersModule : NancyModule
    {
        public CustomersModule(ICustomerStorage customerStorage) 
            : base("/customers")
        {
            // Cors
            After.AddItemToEndOfPipeline(
                (ctx) => ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
            );

            // GET customers
            Get("", parameters =>
            {
                return customerStorage.GetCustomers();
            });

            // GET customer
            Get("/{customerid:int}", parameters =>
            {
                var customerId = (int)parameters.customerid;

                return customerStorage.GetCustomerById(customerId);
            });
        }
    }
}
