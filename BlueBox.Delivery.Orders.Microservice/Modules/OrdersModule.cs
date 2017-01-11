using BlueBox.Delivery.Orders.Microservice.Client;
using BlueBox.Delivery.Orders.Microservice.DataModel;
using BlueBox.Delivery.Orders.Microservice.Model;
using BlueBox.Delivery.Orders.Microservice.Storage;
using Nancy;
using Nancy.ModelBinding;

namespace BlueBox.Delivery.Orders.Microservice.Modules
{
    public class OrdersModule: NancyModule
    {
        public OrdersModule(
            IOrdersStorage orderStorage, 
            ICustomersClient customerClient
        ) 
            : base("/orders")
        {
            // Cors
            After.AddItemToEndOfPipeline(
                (ctx) => ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
            );

            // Note: Get All Orders Availables for Selected Customer
            Get("/{customerid:int}", parameters =>
            {
                var customerId = (int)parameters.customerid; 

                return orderStorage.GetOrdersByCustomerId(customerId);
            });

            // Note: Creates a Order to start request
            Post("", async (parameters, _) =>
            {
                var viewModel = this.Bind<NewOrderModel>();
                var order = Order.Instance();

                // Note: Reques to Customer Microservice customer information to attach to order
                var orderCustomer = await customerClient.GetCustomer(viewModel.CustomerId)
                                                        .ConfigureAwait(false);

                order.SetCustomerInformation(orderCustomer);
                order.ChangeOrderStatus(OrderStatus.NEW);

                orderStorage.Save(order);

                return order;
            });

            // Note: Update the order
            Put("/package", parameters =>
            {
                var viewModel = this.Bind<PackageModel>();

                var order = orderStorage.GetOrderByOrderId(viewModel.OrderId);
                order.SetPackageInformation(
                    new Package(
                        viewModel.PackageWeight, 
                        viewModel.PackageWidth, 
                        viewModel.PackageHeight, 
                        viewModel.DeliveryAddressTo), 
                    viewModel.DeliveryDate
                );

                orderStorage.Save(order);

                return order;
            });
        }
    }
}
