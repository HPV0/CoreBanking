using CoreBanking.API.Filters.IdempotencyFilter;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreBanking.API.Swagger
{
    public sealed class IdempotencyKeyOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var requiresIdempotency = context.MethodInfo
                .GetCustomAttributes(typeof(IdempotentAttribute), true)
                .Any();

            if (!requiresIdempotency)
                return;

            operation.Parameters ??= new List<IOpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Idempotency-Key",
                In = ParameterLocation.Header,
                Required = true,
                Description = "Unique key identifying this request.",
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "uuid"
                }
            });
        }
    }
}
