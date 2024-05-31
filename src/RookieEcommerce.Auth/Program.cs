//Will clean this later after every things checks out

using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RookieEcommerce.Auth;
using RookieEcommerce.Auth.Models;
using RookieEcommerce.Auth.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "Everyone";

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .CreateLogger();

builder.Host.UseSerilog();


//builder.Services.AddCors(options =>
//{
//    //Will set policy later
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("https://localhost:7266",
//                                              "https://localhost:7267",
//                                              "https://localhost:5184",
//                                              ReadConfig.clientUrls["swagger"])
//                                .AllowAnyMethod()
//                                .AllowAnyHeader();
//                      });
//});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<IdentityServerFDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), providerOptions =>
    {
        providerOptions.EnableRetryOnFailure().CommandTimeout(60);
    }));

builder.Services.AddIdentity<BaseApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityServerFDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddReadConfig(builder.Configuration);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

var identityServerBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.Apis)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddAspNetIdentity<BaseApplicationUser>();

identityServerBuilder.AddDeveloperSigningCredential();


//builder.Services.AddAuthentication()
//    .AddGoogle(options =>
//    {
//        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
//        options.ClientId = "copy client ID from Google here";
//        options.ClientSecret = "copy client secret from Google here";
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage();
}

app.UseStaticFiles();


app.UseRouting();
app.UseIdentityServer();
//app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapDefaultControllerRoute();

// Handling seeding from command line arguments
var seed = args.Contains("/seed");
if (seed)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var config = services.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    SeedRole.EnsureSeedData(connectionString, services);

    Log.Information("Done seeding database.");
    return;
}

try
{
    Log.Information("Starting host...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
