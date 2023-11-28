using System.Text.Json.Serialization;
using FastEndpoints.Swagger;

namespace PriceWatcher.Server.Extensions;

static class FastEndpointsExtensions
{
    public static IServiceCollection RegisterFastEndpoints(this IServiceCollection services)
    {
        return services.AddFastEndpoints(opt => opt.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All));
    }

    public static IServiceCollection RegisterSwaggerDocument(this IServiceCollection services)
    {
        return services.SwaggerDocument(opt =>
        {
            opt.ShortSchemaNames = true;
            opt.EnableJWTBearerAuth = false;
            opt.ShowDeprecatedOps = true;
        });
    }

    public static IApplicationBuilder UseRegisteredEndpoints(this IApplicationBuilder app)
    {
        return app.UseFastEndpoints(opt =>
        {
            opt.Endpoints.RoutePrefix = "api";
            opt.Endpoints.ShortNames = true;
            opt.Endpoints.Configurator = ep =>
                ep.Description(d => d.WithName(ep.EndpointType.Name
                    .Replace("Endpoint", string.Empty)));

            opt.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    }
}
