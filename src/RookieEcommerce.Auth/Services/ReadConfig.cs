namespace RookieEcommerce.Auth.Services
{
    //Borrowed from https://github.com/vtoan/fashion-ecom-ns/blob/main/src/Core/ServiceExtensions/ReadConfig.cs
    //:>
    public static class ReadConfig
    {
        public static Dictionary<string, string> clientUrls;

        public static IServiceCollection AddReadConfig(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                clientUrls = new Dictionary<string, string>()
                {
                    ["react"] = configuration["ClientUrls:react"],
                    ["mvc"] = configuration["ClientUrls:mvc"],
                    ["swagger"] = configuration["ClientUrls:swagger"],
                };
            }
            catch (Exception)
            {
                throw new Exception("No define client url");
            }
            return services;
        }
    }
}
