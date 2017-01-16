using Nancy;
using Nancy.TinyIoc;
using BlueBox.Delivery.Customers.Domain;
using BlueBox.Delivery.Customers.Data.Storage;

namespace BlueBox.Delivery.Customers.Microservice
{
    public class Bootstraper: DefaultNancyBootstrapper
    {
        public Bootstraper()
        { }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<ICustomerStorage, CustomerStorage>();
        }
    }
}
