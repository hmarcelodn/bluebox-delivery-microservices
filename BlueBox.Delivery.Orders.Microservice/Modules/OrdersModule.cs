using BlueBox.Delivery.Orders.Domain.Domain;
using BlueBox.Delivery.Orders.Domain.Storage;
using BlueBox.Delivery.Orders.Microservice.Client;
using BlueBox.Delivery.Orders.Microservice.Model;
using Nancy;
using Nancy.ModelBinding;
using Nancy.TinyIoc;

namespace BlueBox.Delivery.Orders.Microservice.Modules
{
    public class OrdersModule: NancyModule
    {
        public OrdersModule(
            IOrderStorage orderStorage, 
            ICustomersClient customerClient
        ) 
            : base("/orders")
        {
            After.AddItemToEndOfPipeline(
                (ctx) => ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
            );            

            Get("/{customerid:int}", parameters =>
            {
                var customerId = (int)parameters.customerid; 

                return orderStorage.GetOrdersByCustomerId(customerId);
            });

            Post("", async (parameters, _) =>
            {
                var newOrderModel = this.Bind<NewOrderModel>();
                var order = Order.Instance();

                var orderCustomer = await customerClient.GetCustomer(newOrderModel.CustomerId)
                                                        .ConfigureAwait(false);

                order.SetCustomerInformation(orderCustomer);
                order.ChangeOrderStatus(OrderStatus.NEW);

                orderStorage.Save(order);

                return order;
            });

            Put("/package", parameters =>
            {
                var packageModel = this.Bind<PackageModel>();

                var order = orderStorage.GetOrderByOrderId(packageModel.OrderId);
                order.SetPackageInformation(
                    new Package(
                        packageModel.PackageWeight,
                        packageModel.PackageWidth,
                        packageModel.PackageHeight,
                        packageModel.DeliveryAddressTo),
                    packageModel.DeliveryDate
                );

                orderStorage.Save(order);

                return order;
            });
        }
    }
}
