using BlueBox.Delivery.APIGateway.Client;
using BlueBox.Delivery.APIGateway.Model;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
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

            this.RequiresAuthentication();

            Get("/customerlist", _ =>
            {
                return customerClient.GetCustomers();
            });

            Get("/orders", _ =>
            {
                var allOrders = orderClient.GetAllOrdersFromOrdersService();

                return Response.AsJson(allOrders);
            });

            Get("/orders/{customerId:int}", async (parameters, _) =>
            {
                var customerId = (int)parameters.customerid;

                var allOrders = await orderClient.GetAllCustomerOrdersFromOrdersService(customerId);

                return Response.AsJson(allOrders);
            });

            Get("/orders/{orderid:Guid}", parameters =>
            {
                var orderId = (Guid)parameters.orderid;

                var orderPackage = orderClient.GetOrderFromOrdersService(orderId);

                return Response.AsJson(orderPackage);
            });

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
