using BlueBox.Delivery.Customers.Domain;
using BlueBox.Delivery.Customers.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueBox.Delivery.Customers.Data.Storage
{
    public class CustomerStorage: ICustomerStorage
    {
        private static IList<Customer> _database = new List<Customer>();

        public CustomerStorage()
        {
            _database.Add(
                new Customer()
                {
                    CustomerId = 1,
                    Address = "Test Address 1",
                    DeliveryAddress = "Delivery Address 1",
                    LastName = "Del Negro",
                    Name = "Hugo Marcelo"
                }
            );

            _database.Add
                (
                    new Customer()
                    {
                        CustomerId = 2,
                        Address = "Test Address 2",
                        DeliveryAddress = "Delivery Address 2",
                        LastName = "The Bot",
                        Name = "Alice"
                    }
                );
        }

        public Customer GetCustomerById(int id)
        {
            var customer = _database.Where(cus => cus.CustomerId == id).FirstOrDefault();

            if (customer != null)
            {
                return customer;
            }

            throw new InvalidOperationException("Customer not found in database.");
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _database.AsEnumerable();
        }
    }
}
