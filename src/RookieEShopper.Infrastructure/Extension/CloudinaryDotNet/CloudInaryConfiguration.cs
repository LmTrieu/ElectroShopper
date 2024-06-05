using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RookieEShopper.Infrastructure.Extension.CloudinaryDotNet
{
    //Ignore for now
    public static class CloudInaryConfiguration
    {
        public static void AddCloudInaryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudName = configuration.GetValue<string>("AccountSettings:CloudName");
            var apiKey = configuration.GetValue<string>("AccountSettings:ApiKey");
            var apiSecret = configuration.GetValue<string>("AccountSettings:ApiSecret");

            if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Please specify Cloudinary account details!");
            }

            services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));
        }
    }
}