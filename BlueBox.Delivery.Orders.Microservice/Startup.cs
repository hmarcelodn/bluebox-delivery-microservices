﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using BlueBox.Delivery.Orders.Microservice.Middleware;

namespace BlueBox.Delivery.Orders.Microservice
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Middleware #1
            app.UseOwin(buildFunc => buildFunc(next => new ConsoleMiddleware(next).Invoke));

            // Middleware #2
            app.UseOwin().UseNancy(opt => opt.Bootstrapper = new Bootstraper());

            loggerFactory.AddConsole().AddDebug();
        }
    }
}
