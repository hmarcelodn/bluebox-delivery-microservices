using BlueBox.Delivery.Orders.Microservice.Aggregates;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Orders.Microservice.Client
{
    public interface ICustomersClient
    {
        Task<OrderCustomer> GetCustomer(int customerid);
    }
}
