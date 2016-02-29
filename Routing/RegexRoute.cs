using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Routing;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Web.Routing
{
    public class RegexRoute : Route
    {
        public Regex Regex { get; private set; }

        public RegexRoute(string regexPattern, IRouteHandler routeHandler)
            : this(regexPattern, null, routeHandler)
        { }

        public RegexRoute(string regexPattern, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(null, defaults, routeHandler)
        {
            this.Regex = new Regex(regexPattern, RegexOptions.Compiled);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string requestUrl = (httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo);
            Match match = this.Regex.Match(requestUrl);

            if (match.Success)
            {
                RouteData data = new RouteData(this, this.RouteHandler);

                if (!this.Defaults.IsNullOrEmpty())
                    foreach (var item in this.Defaults)
                        data.Values[item.Key] = item.Value;

                //Skip the first group, it is the entire match.
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    Group group = match.Groups[i];

                    if (group.Success)
                    {
                        string key = this.Regex.GroupNameFromNumber(i);

                        //Only add named groups.
                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            bool multipleValues = (group.Captures.Count > 1);

                            //Add each group/capture to the route data values.
                            for (int c = 0; c < group.Captures.Count; c++)
                            {
                                Capture capture = group.Captures[c];

                                //If there is more than one value for this key, add the values as a list.
                                if (data.Values.ContainsKey(key))
                                {
                                    //If multiple values then add to the list. Otherwise update the value.
                                    if (multipleValues)
                                        ((List<string>)data.Values[key]).Add(capture.Value);
                                    else
                                        data.Values[key] = capture.Value;
                                }
                                else
                                    data.Values.Add(key, (multipleValues ? (object)(new List<string>() { capture.Value }) : (object)(capture.Value)));
                            }
                        }
                    }
                }

                return data;
            }

            return null;
        }
    }
}