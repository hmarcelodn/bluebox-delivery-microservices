using BlueBox.Delivery.Customers.Microservice.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Serilog;
using System.Threading.Tasks;

namespace BlueBox.Delivery.Customers.Microservice
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<System.Text.Encodings.Web.UrlEncoder>(_ => System.Text.Encodings.Web.UrlEncoder.Default);

            services.AddCors(
                options =>
                    {
                        options.AddPolicy("AllowAll", p => p.AllowAnyOrigin());
                    }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // SeriLog
            var log = ConfigureLogger();

            // Adding Identity Middleware
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5003",
                RequireHttpsMetadata = false,
                ApiName = "api_customers",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                EnableCaching = false,
                AllowedScopes = { "api_customers" }
            });

            // Middlewares
            app.UseOwin(buildFunc =>
            {
                buildFunc(next => GlobalErrorLogging.Middleware(next, log));
                buildFunc(next => CorrelationToken.Middleware(next, log));
                buildFunc(next => RequestLogging.Middleware(next, log));
                buildFunc(next => new MonitoringMiddleware(next, HealthCheck).Invoke);
                buildFunc(next => new ConsoleMiddleware(next).Invoke);
                buildFunc.UseNancy(opt => opt.Bootstrapper = new Bootstraper());                
            });

            app.UseCors("AllowSpecificOrigin");

            loggerFactory.AddConsole().AddDebug();
        }

        // Health Check to Monitoring Middleware
        private async Task<bool> HealthCheck()
        {
            return await Task.FromResult(true);
        }

        private Serilog.ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole(
                    Serilog.Events.LogEventLevel.Verbose,
                    "{NewLiine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                    .CreateLogger();
        }
    }
}
