using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace CRUD.MultiTenant
{
    public class SwaggerTenantHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new System.Collections.Generic.List<OpenApiParameter>();

            // Add X-Tenant-Id header parameter to Swagger UI
            if (!operation.Parameters.Any(p => p.Name == "X-Tenant-Id"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Tenant-Id",
                    In = ParameterLocation.Header,
                    Required = false,
                    Description = "Tenant identifier (use to route requests to tenant-specific resources)",
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}
