using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace QuantumConcepts.Common.Web.HttpHandlers
{
    public class HttpHandlerRouteHandler<T>
       : IRouteHandler where T : IHttpHandler, new()
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new T();
        }
    }
}