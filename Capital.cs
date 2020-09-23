using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Platform
{
    public static  class Capital
    {
        public static async Task Endpoint(HttpContext context)
        {
            string captial = null;
            string country = context.Request.RouteValues["country"] as string;
            switch((country ?? "").ToLower())
            {
                case "uk":
                    captial = "London";
                    break;
                case "france":
                    captial = "Paris";
                    break;
                case "monaco":
                    LinkGenerator generator = context.RequestServices.GetService<LinkGenerator>();
                    string url = generator.GetPathByRouteValues(context,
                    "population", new { city = country });
                    context.Response.Redirect(url);
                    return;
            }

            if(captial != null)
            {
                await context.Response.WriteAsync($"{captial} is the capital of {country}");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
       
    }
}
