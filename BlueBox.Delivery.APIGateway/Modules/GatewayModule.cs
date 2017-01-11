using BlueBox.Delivery.APIGateway.Client;
using BlueBox.Delivery.APIGateway.Model;
using Nancy;
using Nancy.ModelBinding;
using System;

namespace BlueBox.Delivery.APIGateway.Modules
{
    public class GatewayModule : NancyModule
    {
        public GatewayModule(
            ICustomerClient customerClient,
            IOrderClient orderClient)
        {
            // Cors
            After.AddItemToEndOfPipeline(
                (ctx) => ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
            );

            // GET Customers list
            Get("/customerlist", _ =>
            {
                // Call Customers Microservice
                return customerClient.GetCustomers();
            });

            // GET Orders list
            Get("/orders", _ =>
            {
                // Call Orders Microservice to get data
                var allOrders = orderClient.GetAllOrdersFromOrdersService();

                return Response.AsJson(allOrders);
            });

            // GET Orders by customer
            Get("/orders/{customerId:int}", parameters =>
            {
                var customerId = (int)parameters.customerid;
                // Call Orders Microservice to get orders by customer
                var allOrders = orderClient.GetAllCustomerOrdersFromOrdersService(customerId);

                return Response.AsJson(allOrders);
            });

            // GET Order by Guid
            Get("/orders/{orderid:Guid}", parameters =>
            {
                var orderId = (Guid)parameters.orderid;
                var orderPackage = orderClient.GetOrderFromOrdersService(orderId);

                return Response.AsJson(orderPackage);
            });

            // POST new order 
            Post("/orders", async(parameters, _) =>
            {
                var newOrderModel = this.Bind<NewOrderModel>();

                var newOrderGuid = await orderClient.CreateOrderFromOrdersService(newOrderModel.CustomerId);

                await orderClient.UpdateOrderWithPackageFromOrdersService(newOrderGuid, newOrderModel);

                return 200;
            });
        }
    }
}
