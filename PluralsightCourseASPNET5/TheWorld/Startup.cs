using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace TheWorld
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(conf => {
                conf.MapRoute(
                  name: "Default",
                  template: "{controller}/{action}/{id?}",
                  defaults: new{ controller = "App", action = "Index" } 
                );
            });
            //app.UseIISPlatformHandler();

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync($"Hello World! {context.Request.Path}");
            // });
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
