using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using RookieEShopper.Api;
using RookieEShopper.Application;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentityApiEndpoints<BaseApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapIdentityApi<BaseApplicationUser>();

app.MapControllers();

app.Run();
