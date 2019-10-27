using System;
using BotsController.Models;
using BotsController.Models.Bots;
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            new ScrumPokerBot().GetBotClient(Environment.GetEnvironmentVariable("SCRUM_POKER_BOT_TOKEN"));
            new GrishaBot().GetBotClient(Environment.GetEnvironmentVariable("GRISHA_BOT_TOKEN"));

        }
    }
}