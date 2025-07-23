using Asp.Versioning.ApiExplorer;
using FDA.Backend.Application;
using FDA.Backend.Extensions;
using FDA.Backend.Infrastructure;
using FDA.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add base services to the container.
var databaseFilePath = builder.Configuration.GetValue<string>("FDASettings:DatabasePath");

builder.Services.AddDbContext<FDAContext>(o
    => o.UseSqlite($"Data Source={databaseFilePath}"));

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();
builder.Services.AddSerilog();

// Add Authorization / Authentication
//builder.Services.AddIdentity(builder.Configuration);

// Add app specific services
//builder.Services.AddServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
//builder.Services.AddReportCapability();

// Add Open API explorer
builder.Services.AddSwaggerWithIdentity();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
}).AddApiExplorer(options => options.GroupNameFormat = "'v'VV");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseIdentitySwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
}

#if DEBUG
// Allow cors with any origin
app.UseCors(c => c
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
    .WithExposedHeaders("Content-Disposition")
);
#endif

#if RELEASE
app.UseHttpsRedirection();
#endif 

//app.UseReportServer();

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseStaticFiles();
//app.UseSession();
//app.UseRouting();
//app.UseReportServerRoutes();

//app.UseWebReportViewer();

//app.MapDefaultControllerRoute();
//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

app.MapControllers();
app.MigrateDatabase();
app.Run();

//app.MapGet("/", () => "Hello World!");
