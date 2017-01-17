using BlueBox.Delivery.Orders.Data.Storage;
using BlueBox.Delivery.Orders.Domain.Storage;
using BlueBox.Delivery.Orders.Microservice.Client;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using Serilog;

namespace BlueBox.Delivery.Orders.Microservice
{
    public class Bootstraper: DefaultNancyBootstrapper
    {
        private readonly ILogger _log;

        public Bootstraper(ILogger log)
        {
            this._log = log;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register(_log);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var correlationToken = context.GetOwinEnvironment()["correlationToken"] as string;       

            container.Register<ICustomersClient>(new CustomersClient(new HttpClientFactory(correlationToken)));
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IOrderStorage, OrderStorage>();
            container.Register<ICustomersClient, CustomersClient>();
        }
    }
}
