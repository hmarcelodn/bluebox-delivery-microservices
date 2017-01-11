using BlueBox.Delivery.APIGateway.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueBox.Delivery.APIGateway.Client
{
    public interface IOrderClient
    {
        Task<Guid> CreateOrderFromOrdersService(int customerId);

        Task UpdateOrderWithPackageFromOrdersService(Guid orderId, NewOrderModel orderModel);

        Task<object> GetOrderFromOrdersService(Guid orderId);

        Task<IEnumerable<object>> GetAllOrdersFromOrdersService();

        Task<IEnumerable<object>> GetAllCustomerOrdersFromOrdersService(int customerid);
    }
}
