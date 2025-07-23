using FDA.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace FDA.Backend.Extensions;

/// <summary>
/// Extensions for the database migrations
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Do the migrations on the internal database
    /// </summary>
    /// <param name="app">The webapplication instance</param>
    /// <returns>The webapplication instance</returns>
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateScope().ServiceProvider;
        var db = scope.GetRequiredService<FDAContext>();
        db.Database.Migrate();

        //var logService = scope.GetRequiredService<ILogService>();
        //logService.Startup("Startup: {0}", DateTime.Now.ToString());

        return app;
    }
}

