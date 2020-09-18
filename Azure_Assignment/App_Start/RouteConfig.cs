using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Azure_Assignment
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product By Category",
                url: "category-{id}",
                defaults: new { controller = "Home", action = "Shop", id = UrlParameter.Optional},
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Login", action = "Logout", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
               name: "Register",
               url: "register",
               defaults: new { controller = "Register", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Azure_Assignment.Controllers" }
           );

            routes.MapRoute(
                name: "Cart",
                url: "cart",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Add To Cart",
                url: "add-to-cart",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "Azure_Assignment.Controllers" }
            );
        }
    }
}
