using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibOwin;
using Serilog;
using Serilog.Context;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;


namespace BlueBox.Delivery.Customers.Microservice.Middleware
{
    public class CorrelationToken
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                Guid correlationToken;

                var owinContext = new OwinContext(env);

                if (!(owinContext.Request.Headers["Correlation-Token"] != null
                    && Guid.TryParse(owinContext.Request.Headers["Correlation-Token"], out correlationToken)))
                    correlationToken = Guid.NewGuid();

                owinContext.Set("correlationToken", correlationToken.ToString());

                using (LogContext.PushProperty("correlationToken", correlationToken))
                    await next(env);
            };
        }
    }

    public class RequestLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                var owinContext = new OwinContext(env);
                log.Information(
                  "Incoming request: {@Method}, {@Path}, {@Headers}",
                  owinContext.Request.Method,
                  owinContext.Request.Path,
                  owinContext.Request.Headers);
                await next(env);
                log.Information(
                  "Outgoing response: {@StatucCode}, {@Headers}",
                   owinContext.Response.StatusCode,
                   owinContext.Response.Headers);
            };
        }
    }

    public class GlobalErrorLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                try
                {
                    await next(env);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Unhandled exception");
                }
            };
        }
    }
}
