using BlueBox.Delivery.Orders.Domain.Domain;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Orders.Microservice.Client
{
    public interface ICustomersClient
    {
        Task<Customer> GetCustomer(int customerid);
    }
}
