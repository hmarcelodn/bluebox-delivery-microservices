using Nancy;
using BlueBox.Delivery.Customers.Domain;
using Nancy.Security;

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

            // Authentication
            this.RequiresAuthentication();

            Get("", parameters =>
            {
                return customerStorage.GetCustomers();
            });

            Get("/{customerid:int}", parameters =>
            {
                var customerId = (int)parameters.customerid;

                return customerStorage.GetCustomerById(customerId);
            });
        }
    }
}
