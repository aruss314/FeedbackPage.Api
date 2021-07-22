using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FeedbackPage.Api.Extensions
{
    public sealed class ArussSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IWebHostEnvironment _environment;

        public ArussSwaggerUIOptions(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_environment.ApplicationName} V1");
        }
    }
}
