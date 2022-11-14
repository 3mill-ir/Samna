using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BigMill
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Sitemap",
               "sitemap.xml",
               new { controller = "Sitemap", action = "SitemapXml" }
               );
            routes.MapRoute(
                   "NewsDetail",
                  "News/{action}/{PostId}/{PostTittle}",
                    new { controller = "News", action = "AkhbarOmumi_Detail", PostId = UrlParameter.Optional, PostTittle = UrlParameter.Optional }, namespaces: new[] { "BigMill.Controllers" }
                );
            routes.MapRoute(
              "ParliamentDetail",
             "Parliament/{action}/{PostId}/{PostTittle}",
               new { controller = "Parliament", action = "ViewCorrespondence_Detail", PostId = UrlParameter.Optional, PostTittle = UrlParameter.Optional }, namespaces: new[] { "BigMill.Controllers" }
           );
            routes.MapRoute(
        "StaticViews",
       "Static/{action}/{FileName}",
         new { controller = "Static", action = "StaticView", FileName = UrlParameter.Optional }, namespaces: new[] { "BigMill.Controllers" }
     );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "BigMill.Controllers" }
            );
        }
    }
}
