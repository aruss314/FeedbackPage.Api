using FeedbackPage.Api.Swagger.DocumentFilters;
using FeedbackPage.Api.Swagger.OperationFilters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace FeedbackPage.Api.Extensions
{
    public sealed class ArussSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IWebHostEnvironment _environment;
        public ArussSwaggerGenOptions(IConfiguration configuraiton, IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void Configure(SwaggerGenOptions options)
        {
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo { Title = $"{_environment.EnvironmentName}-{_environment.ApplicationName}, Location: ${Environment.MachineName}", Version = "v1", Description = $"Api for Aruss feedback", Contact = new OpenApiContact { Name = "Aruss" } });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.DocumentFilter<HealthCheckFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        }
    }
}
