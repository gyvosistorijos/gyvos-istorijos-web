using Hintme.Controllers;
using Hintme.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hintme
{
    public class Startup
    {
        private readonly IConfigurationRoot _config;
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("config.json")
              .AddEnvironmentVariables();

            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);
            services.AddMvc(config =>
            {
                config.ModelBinderProviders.Insert(0, new CoordinatesModelBinderProvider());
            });
            services.AddSingleton<IHintRepository, GoogleSheetsRepository>();

        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" });
            });
            app.UseStaticFiles();
        }
    }
}
