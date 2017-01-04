using System;
using System.Collections.Generic;
using BlueBox.Delivery.Customers.Microservice.Model;

namespace BlueBox.Delivery.Customers.Microservice.Storage
{
    public class CustomerStorage : ICustomerStorage
    {
        private static readonly Dictionary<int, Customer> _database = new Dictionary<int, Customer>();

        public CustomerStorage()
        {
            _database.Add(
                1,
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
                    2,
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
            if (_database.ContainsKey(id))
            {
                return _database[id];
            }

            throw new InvalidOperationException("Customer not found in database.");
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }
    }
}
