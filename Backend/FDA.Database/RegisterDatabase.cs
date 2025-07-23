//using LLPrintServer.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FDA.Database;

public class RegisterDatabase
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration, string databasePath)
    {
        //services.AddDbContext<LLPrintServerContext>(o => o.UseSqlite("Data Source=../../../LLPrintServer.Backend/LLPrintServer.db"));
    }
}



