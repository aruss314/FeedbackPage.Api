using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace FeedbackPage.Api.Swagger.DocumentFilters
{
    // Review this
    public class HealthCheckFilter : IDocumentFilter
    {
        public const string HealthCheckEndpoint = @"/health";

        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
        {
            var pathItem = new OpenApiPathItem();

            var operation = new OpenApiOperation();
            operation.Tags.Add(new OpenApiTag { Name = "ApiHealth" });

            var properties = new Dictionary<string, OpenApiSchema>();
            properties.Add("status", new OpenApiSchema() { Type = "string" });
            properties.Add("errors", new OpenApiSchema() { Type = "array" });

            var response = new OpenApiResponse();
            response.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = true,
                    Properties = properties,
                }
            });

            operation.Responses.Add("200", response);
            pathItem.AddOperation(OperationType.Get, operation);
            openApiDocument?.Paths.Add(HealthCheckEndpoint, pathItem);
        }
    }
}
