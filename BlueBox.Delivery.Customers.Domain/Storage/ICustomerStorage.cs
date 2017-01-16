using BlueBox.Delivery.Customers.Domain.Domain;
using System.Collections.Generic;

namespace BlueBox.Delivery.Customers.Domain
{
    public interface ICustomerStorage
    {
        Customer GetCustomerById(int id);

        IEnumerable<Customer> GetCustomers();
    }
}
