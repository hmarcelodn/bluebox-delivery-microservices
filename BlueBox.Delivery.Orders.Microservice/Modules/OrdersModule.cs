using BlueBox.Delivery.Orders.Microservice.Aggregates;
using BlueBox.Delivery.Orders.Microservice.Client;
using BlueBox.Delivery.Orders.Microservice.Model;
using BlueBox.Delivery.Orders.Microservice.Storage;
using Nancy;
using Nancy.ModelBinding;
using System;

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
                var viewModel = this.Bind<PackageViewModel>();

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
