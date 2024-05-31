using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using RookieEShopper.CustomerFrontend.Services;
using RookieEShopper.CustomerFrontend.Services.Category;
using RookieEShopper.CustomerFrontend.Services.Product;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookie";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookie")
    .AddOpenIdConnect("oidc", options =>
     {
         options.Authority = "https://localhost:8899";

         options.ClientId = "mvc";
         options.ClientSecret = "secret";
         options.ResponseType = "code";

         options.Scope.Add("profile");
         options.Scope.Add("customer.read");
         options.Scope.Add("customer.write");

         options.GetClaimsFromUserInfoEndpoint = true;

         options.SaveTokens = true;

         options.Events = new OpenIdConnectEvents
         {

             OnTokenResponseReceived = ctx =>
             {
                 var accessToken = ctx.TokenEndpointResponse.AccessToken;

                 var idToken = ctx.TokenEndpointResponse.IdToken;

                 return Task.CompletedTask;
             },
         };
     });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccessTokenHandler>();

builder.Services.AddHttpClient("apiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7265");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<AccessTokenHandler>();

// Add services to the container.
//builder.Services.AddHttpClient<ICategoryClient, CategoryClient>();
//builder.Services.AddHttpClient<IProductClient, ProductClient>();

builder.Services.AddScoped<ICategoryClient, CategoryClient>();
builder.Services.AddScoped<IProductClient, ProductClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
