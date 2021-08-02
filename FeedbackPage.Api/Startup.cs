using System.Linq;
using System.Text.Json;
using FeedbackPage.Api.Extensions;
using FeedbackPage.Dal.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FeedbackPage.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "Aruss_Feedback_")
                .AddJsonFile(@"c:\temp\.FeedbackPage.Api\appsettings.json", true);

            Configuration = builder.Build();

            Environment = env;
        }

        private IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddWinHistoryRepository(Configuration);

            // Swagger
            services.AddSingleton<IConfigureOptions<JsonOptions>, JsonSerializationOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, ArussSwaggerUIOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ArussSwaggerGenOptions>();
            services.AddSwaggerGen();

            // Health checks
            services.AddHealthChecks();

            // CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    // Todo: put these in config vars
                    //.WithOrigins(config.GetSection("AllowedHosts").Value);
                    .WithOrigins("http://aruss-feedback-wfe.atriarch.systems", "https://aruss-feedback-wfe.atriarch.systems", "http://localhost:3000");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecks("/health/dependency", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("dependency"),
                    ResponseWriter = async (context, report) =>
                    {
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                            {
                                status = report.Status.ToString(),
                                health = report.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
                            },
                            new JsonSerializerOptions { WriteIndented = true });
                        await context.Response.WriteAsync(result);
                    }
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
                app.UseSwagger();
                app.UseSwaggerUI();
            });
        }
    }
}
