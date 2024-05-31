using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using RookieEShopper.Api;
using RookieEShopper.Api.Middlewares;
using RookieEShopper.Application;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent;


var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "ReactAdmin";

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

builder.Services.AddIdentityApiEndpoints<BaseApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("swagger");
        options.OAuthClientSecret("secret");
        options.OAuthUsePkce();
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RookieEcommerce API v1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

//app.MapIdentityApi<BaseApplicationUser>();

app.MapControllers();

app.Run();