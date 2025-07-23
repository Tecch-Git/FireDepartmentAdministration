using FDA.Backend.Application.Services;

namespace FDA.Backend.Application;

/// <summary>
/// Extension for adding needed sevices in the application directories
/// </summary>
public static class RegisterApplication
{
    /// <summary>
    /// Add services to the service collection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        return services;
    }
}
