using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using QuantumConcepts.Common.Web.HttpHandlers;

namespace QuantumConcepts.Common.Web.Extensions
{
    public static class WebExtensions
    {
        public static void MapHttpHandler<T>(this RouteCollection routes, string url)
            where T : IHttpHandler, new()
        {
            MapHttpHandler<T>(routes, null, url, null, null);
        }

        public static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints)
            where T : IHttpHandler, new()
        {
            Route route = new Route(url, new HttpHandlerRouteHandler<T>());

            route.Defaults = new RouteValueDictionary(defaults);
            route.Constraints = new RouteValueDictionary(constraints);
            routes.Add(name, route);
        }
    }
}