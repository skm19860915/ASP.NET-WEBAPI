using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using rmxwebapi.Models;

namespace rmxwebapi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "ReloadCache",
               url: "ReloadCache",
               defaults: new { controller = "ReloadCache", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
