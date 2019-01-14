using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "product",
             url: "product",
             defaults: new { controller = "Home", action = "MasterGroup" });
 routes.MapRoute(
             name: "download",
             url: "download",
             defaults: new { controller = "Home", action = "Download" });

 routes.MapRoute(
             name: "store",
             url: "store",
             defaults: new { controller = "Home", action = "Stores" });
     
 routes.MapRoute(
             name: "checkout",
             url: "checkout",
             defaults: new { controller = "Checkout", action = "Create" });
            routes.MapRoute(
                        name: "admin store",
                        url: "admin/store",
                        defaults: new { controller = "Store", action = "Index" });

            routes.MapRoute(
            name: "product-code",
            url: "product/{code}",
            defaults: new { controller = "Home", action = "Product", code = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
