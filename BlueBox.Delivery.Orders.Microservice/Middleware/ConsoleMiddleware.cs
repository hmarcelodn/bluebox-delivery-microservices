using LibOwin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace BlueBox.Delivery.Orders.Microservice.Middleware
{
    public class ConsoleMiddleware
    {
        private AppFunc next;

        public ConsoleMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext();
            var method = context.Request.Method;
            var path = context.Request.Path;

            Console.WriteLine($"Got request: {method} {path}");

            return next(env);
        }
    }
}
