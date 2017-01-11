using BlueBox.Delivery.Customers.Microservice.DataModel;
using System.Collections.Generic;

namespace BlueBox.Delivery.Customers.Microservice.Storage
{
    public interface ICustomerStorage
    {
        Customer GetCustomerById(int id);

        IEnumerable<Customer> GetCustomers();
    }
}
