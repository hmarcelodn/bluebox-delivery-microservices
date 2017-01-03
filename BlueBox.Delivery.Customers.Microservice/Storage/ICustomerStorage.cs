using BlueBox.Delivery.Customers.Microservice.Model;

namespace BlueBox.Delivery.Customers.Microservice.Storage
{
    public interface ICustomerStorage
    {
        Customer GetCustomerById(int id);
    }
}
