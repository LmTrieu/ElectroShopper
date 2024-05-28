using Microsoft.AspNetCore.Identity;
using RookieEShopper.Api;
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
                          policy.WithOrigins("https://localhost:7266",
                                              "https://localhost:7267")
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.MapIdentityApi<BaseApplicationUser>();

app.MapControllers();

app.Run();