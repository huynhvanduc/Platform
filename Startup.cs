using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> msOptions)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseMiddleware<WeatherMiddleware>();

            IResponseFormatter formatter = new TextResponseFormatter();
            app.Use(async (context, next) => {
                if(context.Request.Path == "/middleware/function")
                {
                    await TypeBroker.Formatter.Format(context, "Middleware Function: It is snowing in Chicago");
                }
                else
                {
                    await next();
                }
            });

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);

                endpoints.MapGet("/endpoint/function", async context => {
                    await TypeBroker.Formatter.Format(context, "Endpoint Function: Is is sunny in LA");
                });
            });
            app.Use(async (context, next) => {
                await context.Response.WriteAsync("Terminal Middleware Reached");
            });
        }
    }
}
