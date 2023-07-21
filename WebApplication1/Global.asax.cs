using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class WebApiApplication : HttpApplication
    {
        private TracerProvider tracerProvider;

        void Application_Start(object sender, EventArgs e)
        {
            this.tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddAspNetInstrumentation()
                .AddConsoleExporter()
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService("my-service-name"))
                .AddOtlpExporter(options =>
                {
                    options.Endpoint =
                    new Uri("http://10.10.70.112:4330/v1/traces");
                })
                .Build();
        }

        void Application_End()
        {
            this.tracerProvider?.Dispose();
        }
    }
}
