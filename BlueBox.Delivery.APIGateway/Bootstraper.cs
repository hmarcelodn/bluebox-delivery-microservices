using BlueBox.Delivery.APIGateway.Client;
using MicroservicesNET.Platform;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using Serilog;

namespace BlueBox.Delivery.APIGateway.Microservice
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

            container.Register<ICustomerClient>(new CustomerClient(new HttpClientFactory(correlationToken)));
            container.Register<IOrderClient>(new OrderClient(new HttpClientFactory(correlationToken)));
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<ICustomerClient, CustomerClient>();
            container.Register<IOrderClient, OrderClient>();
        }
    }
}
