using FDA.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FDA.Backend.Infrastructure;

/// <summary>
/// Extension for adding needed services in the infrastructure directories
/// </summary>
public static class RegisterInfrastructure
{
    /// <summary>
    /// Add services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();
        return services;
    }
}