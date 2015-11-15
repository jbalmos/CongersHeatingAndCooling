using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CongerHeatingAndCooling
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "SubmissionActions",
                url: "Success",
                defaults: new { controller = "Company", action = "Success" }
            );
            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Company", action = "Home", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "About",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Company", action = "About", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Products",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Company", action = "Products", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Services",
                url: "Company/Services",
                defaults: new { controller = "Company", action = "Services" }
            );
            routes.MapRoute(
                name: "Testimonials",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Company", action = "Testimonials", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Support",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Support", action = "ContactUs", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Company", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}