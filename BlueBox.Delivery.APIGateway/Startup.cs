using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;

namespace BlueBox.Delivery.APIGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<System.Text.Encodings.Web.UrlEncoder>(_ => System.Text.Encodings.Web.UrlEncoder.Default);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5003",
                RequireHttpsMetadata = false,
                ApiName = "api1",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseOwin().UseNancy();
        }
    }
}
