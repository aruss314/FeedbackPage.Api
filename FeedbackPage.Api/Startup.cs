using System.Linq;
using System.Text.Json;
using FeedbackPage.Api.Extensions;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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
                    .WithOrigins("http://aruss-feedback-wfe.atriarch.systems", "https://aruss-feedback-wfe.atriarch.systems", "http://localhost:4200");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
