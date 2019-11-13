using System;
using BotsController.Core.Bots;
using BotsController.Core.Interfaces;
using BotsController.DAL;
using BotsController.Models;
using BotsController.Models.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace BotsController
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
            services.AddSingleton(db => new LiteDbContext(Environment.GetEnvironmentVariable("DATA_BASE_FILE")));
            services.AddSingleton<IRepository<Voice>, LiteDbRepository<Voice>>();
            services.AddSingleton<ScrumPokerBot, ScrumPokerBot>();
            services.AddSingleton<GrishaBot, GrishaBot>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            app
                .UseRouting()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseCors("CorsPolicy")
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });

            if (env.EnvironmentName == "Production")
            {
                var options = new RewriteOptions()
                    .AddRedirectToHttpsPermanent();
                app.UseRewriter(options);
            }

            services.GetService(typeof(GrishaBot));
            services.GetService(typeof(ScrumPokerBot));

        }
    }
}