using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace PE.API.Extensions
{
    public static class GeneralExtensions
    {
        public static string FromGuidToBase64String(this Guid input)
        {
            var converted = Convert.ToBase64String(input.ToByteArray());
            return converted.Remove(converted.Length - 2);

        }

        public static Guid FromBase64StringToGuid(this string input)
        {
            return new Guid(Convert.FromBase64String(input + "=="));
        }

        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return string.Empty;

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
