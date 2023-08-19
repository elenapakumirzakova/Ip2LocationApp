namespace Ip2LocationApp.Extensions;
public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddConfig(
         this IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<IpStackOptions>()
            .Bind(config.GetSection(IpStackOptions.Key));

        services
            .AddOptions<CacheOptions>()
            .Bind(config.GetSection(CacheOptions.Key));

        return services;
    }

    public static IServiceCollection AddMyDependencyGroup(
         this IServiceCollection services)
    {
        // Add services to the container.
        services.AddHttpClient<IIpStackHttpClient, IpStackHttpClient>(
            client =>
            {
                // Set the base address of the typed client.
                client.BaseAddress = new Uri("http://api.ipstack.com/");
            });

        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
}
