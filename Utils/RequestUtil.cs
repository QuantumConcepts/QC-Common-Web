using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Collections;
using System.Globalization;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Web.Utils
{
    public static class RequestUtil
    {
        public static string GetParameter(HttpContext context, string name)
        {
            return (context.Request.QueryString[name] ?? Convert.ToString(context.Request.RequestContext.RouteData.Values[name]));
        }

        public static string[] GetParameters(HttpContext context, string name)
        {
            string[] values = context.Request.QueryString.GetValues(name);

            if (values == null && context.Request.RequestContext.RouteData.Values.ContainsKey(name))
            {
                object temp = context.Request.RequestContext.RouteData.Values[name];

                if (temp is string)
                    values = new string[] { (string)temp };
                else if (temp is IEnumerable)
                    values = ((IEnumerable)temp).Cast<object>().Select(o => o.ToString()).ToArray();
            }

            return values;
        }

        public static bool ValidateAndExtractParameter(HttpContext context, string name, ref string value)
        {
            value = GetParameter(context, name);

            if (string.IsNullOrEmpty(value))
                return false;

            return true;
        }

        public static bool ValidateAndExtractIntParameter(HttpContext context, string name, ref int value)
        {
            string valueString = null;

            if (ValidateAndExtractParameter(context, name, ref valueString))
            {
                if (!int.TryParse(valueString, out value))
                    return false;
            }
            else
                return false;

            return true;
        }

        public static bool ValidateAndExtractNullableIntParameter(HttpContext context, string name, ref int? value)
        {
            int intValue = 0;

            if (ValidateAndExtractIntParameter(context, name, ref intValue))
            {
                value = intValue;

                return true;
            }

            return false;
        }

        public static bool ValidateAndExtractBoolParameter(HttpContext context, string name, ref bool value)
        {
            string valueString = null;

            if (ValidateAndExtractParameter(context, name, ref valueString))
            {
                if (!bool.TryParse(valueString, out value))
                    return false;
            }
            else
                return false;

            return true;
        }

        public static bool ValidateAndExtractNullableBoolParameter(HttpContext context, string name, ref bool? value)
        {
            bool boolValue = false;

            if (ValidateAndExtractBoolParameter(context, name, ref boolValue))
            {
                value = boolValue;

                return true;
            }

            return false;
        }

        public static bool ValidateAndExtractEnumParameter<T>(HttpContext context, string name, ref T value)
            where T : struct
        {
            string valueString = null;

            if (ValidateAndExtractParameter(context, name, ref valueString))
            {
                if (!Enum.TryParse<T>(valueString, out value))
                    return false;
            }
            else
                return false;

            return true;
        }

        public static bool ValidateAndExtractNullableEnumParameter<T>(HttpContext context, string name, ref T? value)
            where T : struct
        {
            T enumValue = default(T);

            if (ValidateAndExtractEnumParameter<T>(context, name, ref enumValue))
            {
                value = enumValue;

                return true;
            }

            return false;
        }

        public static CultureInfo GetCulture(this HttpRequest request)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            if (request != null)
            {
                string language = request.UserLanguages.FirstOrDefault();

                if (!language.IsNullOrEmpty())
                    cultureInfo = CultureInfo.CreateSpecificCulture(language.ToLowerInvariant());
            }

            return cultureInfo;
        }

        public static RegionInfo GetRegion(this HttpRequest request)
        {
            return (request.GetCulture().ValueOrDefault(o => new RegionInfo(o.LCID)) ?? RegionInfo.CurrentRegion);
        }
    }
}