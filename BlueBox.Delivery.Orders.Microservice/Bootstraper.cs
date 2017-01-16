using Nancy;
using Nancy.TinyIoc;
using BlueBox.Delivery.Orders.Domain.Storage;
using BlueBox.Delivery.Orders.Data.Storage;

namespace BlueBox.Delivery.Orders.Microservice
{
    public class Bootstraper: DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IOrderStorage, OrderStorage>();
        }
    }
}
