using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Web", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "display",
                url: "{controller}/display/{ip}/{port}",
                defaults: new { controller = "Web", action = "display", id = UrlParameter.Optional }
            );

            //routes.MapRoute(name:"display", url:"display/{ip}/{port}", defaults: new
            //{ Controller = "Web", action ="display"});
        }
    }
}
