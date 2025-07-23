
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Asp.Versioning.ApiExplorer;

namespace FDA.Backend.Extensions;

/// <summary>
/// Extensions for adding and configuring the swagger api documentation tool.
/// This configuration uses versioned api and enables bearer authentication
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Removes the version parameter from an endpoint, if the route contains one
    /// </summary>
    public class VersionRemover : IOperationFilter
    {
        /// <summary>
        /// Apply the filter on the given operation
        /// </summary>
        /// <param name="operation">The operation to handle</param>
        /// <param name="_">unused</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext _)
        {
            try
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
            catch { }
        }
    }

    /// <summary>
    /// Replaces the version parameter in the route to call the correct endpoint
    /// </summary>
    public class VersionReplacer : IDocumentFilter
    {
        /// <summary>
        /// Apply the filter on the given document
        /// </summary>
        /// <param name="swaggerDoc">The sawgger document</param>
        /// <param name="_">unsused</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext _)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }

    /// <summary>
    /// Configure the api document with meta informations
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        /// <summary>
        /// Configure the swagger generator to use the versioning scheme
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        /// <summary>
        /// Configure the meta informations for each versioned api
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "FireDepartmentAdiministration",
                Version = description.ApiVersion.ToString(),
                Description = "Administration application for fire departments",
            };

            if (description.IsDeprecated)
                info.Description += "<br><b><h3 style='color: red important!;'> This API version has been deprecated.</h3></b>";

            return info;
        }
    }

    /// <summary>
    /// Add swagger services to the dependency container and configure the authentication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddSwaggerWithIdentity(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddCors(o =>
            o.AddDefaultPolicy(b =>
                b.AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowAnyOrigin()
            )
        );

        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(o =>
        {
            o.OperationFilter<VersionRemover>();
            o.DocumentFilter<VersionReplacer>();
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            o.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        return services;
    }

    /// <summary>
    /// Add the swagger middleware to the application
    /// </summary>
    /// <param name="app">The application instance</param>
    /// <param name="provider">API Versioning provider</param>
    public static void UseIdentitySwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        });
    }
}