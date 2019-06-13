using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;

namespace rmxwebapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Cache",
                routeTemplate: "Cache",
                defaults: new { controller = "Cache", action = "Get" }
            );

            config.Routes.MapHttpRoute(
                name: "rmxrecorder-authentication",
                routeTemplate: "RMXRecorder/Auth/{id}",
                defaults: new { controller = "Auth", action = "Get", id = string.Empty }
            );

            config.Routes.MapHttpRoute(
                name: "rmxrecorder-rentals",
                routeTemplate: "RMXRecorder/RentalVehicle/{id}",
                defaults: new { controller = "RentalVehicle", action = "Get", id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
