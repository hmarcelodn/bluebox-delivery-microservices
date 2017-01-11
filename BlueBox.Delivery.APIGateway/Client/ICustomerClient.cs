using BlueBox.Delivery.APIGateway.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueBox.Delivery.APIGateway.Client
{
    public interface ICustomerClient
    {
        Task<IEnumerable<CustomerModel>> GetCustomers();
    }
}
