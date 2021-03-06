﻿using System;
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
                url: "{action}/{id}",
                defaults: new { controller = "Web", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "display",
                url: "display/{ip}/{port}",
                defaults: new { controller = "Web", action = "display", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "timesPerSecond",
                url: "display/{ip}/{port}/{times}",
                defaults: new { controller = "Web", action = "timesPerSecond", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "saveToFile",
                url: "save/{ip}/{port}/{rate}/{time}/{fileName}",
                defaults: new { controller = "Web", action = "saveToFile", id = UrlParameter.Optional }
            );
            //routes.MapRoute(name:"display", url:"display/{ip}/{port}", defaults: new
            //{ Controller = "Web", action ="display"});
        }
    }
}
